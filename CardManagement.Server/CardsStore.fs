module CardManagement.Server.CardsStore

open CardManagement.Shared.Core
open CardManagement.Shared.Types
open CardManagement.Database.CardsRepository
open CardManagement.Database.UsersRepository
open CardManagement.Server.RemotingUtils
open CardManagement.Infrastructure.CardActions
open CardManagement.Database.TransactionsRepository

let private getUserCards userId () = async {
    try
        let! cards = getCards userId
        return Ok cards
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private createCard userId typeCard = async {
    try
        let! user = tryFindUserById userId
        if user.IsNone then return Error { Message = "User with this id not found" }
        else
            let availableTypesCard = getIsAvailableCard user.Value
            let isExistTypeCard = availableTypesCard |> Seq.contains typeCard
            match isExistTypeCard with
            | false -> return Error { Message = "Sorry, We can't create this card for you" }
            | true ->
                let newCard = buildCard user.Value typeCard 1000
                do! newCard |> saveCard |> Async.Ignore
                return Ok newCard;
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private createTransaction userId transactionInput = async {
    try
        let! isExistCardRecipient = tryFindCardByCode transactionInput.Code
        let! cardsSender = getCards userId 
        let isExistCardSender = cardsSender |> Seq.tryFind (fun v -> v.Id = transactionInput.CardIdSender)
        if transactionInput.Amount < 1 then return Error { Message = "Not valid amount" } 
        else if isExistCardSender.Value.Code = transactionInput.Code then return Error { Message = "Not available transaction" }
        else if isExistCardRecipient.IsNone then return Error { Message = "Error card with this code not found" }
        else if isExistCardSender.IsNone then return Error { Message = "Server error" } else
        let recipientId = isExistCardRecipient.Value.UserId
        let! transactionsCardSender = getTransactionsByCardId isExistCardSender.Value.Id
        let cardSenderWithTransactions = { isExistCardSender.Value with Transactions = transactionsCardSender }
        let isSuccessTransaction = processPayment cardSenderWithTransactions transactionInput.Amount recipientId transactionInput.Message
        match isSuccessTransaction with
        | Error error -> return Error { Message = error.Message }
        | Ok cardWithUpdateTransactions ->
            let newTransactionBySender = cardWithUpdateTransactions.Transactions |> Seq.last
            do! newTransactionBySender |> saveTransaction |> Async.Ignore
            do! updateCardBalanceById cardWithUpdateTransactions.Id cardWithUpdateTransactions.Balance |> Async.Ignore
            do! updateCardBalanceById isExistCardRecipient.Value.Id (isExistCardRecipient.Value.Balance + transactionInput.Amount) |> Async.Ignore
            return Ok newTransactionBySender
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getMyTransactions userId cardId = async {
    try
        let! cards = getCards userId 
        let isExistCard = cards |> Seq.tryFind (fun v -> v.Id = cardId)
        match isExistCard with
        | None -> return Error { Message = "It's not your card" }
        | Some card ->
            let! transactionsByCard = getTransactionsByCardId card.Id
            let! transactionsToUserId = getTransactionsToUserId userId card.Id
            let allTransactions = Seq.concat [transactionsByCard; transactionsToUserId]
            return Ok allTransactions
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getCardsStoreImplementation ctx: ICardsStore =
    let userId = getUserIdFromHttpContext ctx
    {
        Get = getUserCards userId
        Create = createCard userId
        CreateTransaction = createTransaction userId
        GetTransactions = getMyTransactions userId 
    }

