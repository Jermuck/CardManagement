module CardManagement.Server.CardsStore

open CardManagement.Shared
open CardManagement.Database
open CardManagement.Server
open CardManagement.Infrastructure
open Core
open Types
open CardsRepository
open UsersRepository
open RemotingUtils
open CardActions
open TransactionsRepository

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
        if user.IsNone then return Error { Message = "User with this id not found" } else
        let availableTypesCard = getIsAvailableCard user.Value
        let isExistTypeCard = availableTypesCard |> Seq.contains typeCard
        match isExistTypeCard with
        | false -> return Error { Message = "Sorry, We can't create this card for you" }
        | true ->
            let newCard = buildCard user.Value typeCard 1000
            do! newCard |> saveCard |> Async.Ignore
            return Ok newCard
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private createTransaction userId transactionInput = async {
    try
        let! cardsSender = getCards userId
        let isExistCardSender = cardsSender |> Seq.tryFind (fun v -> v.Id = transactionInput.CardIdSender)
        let! isExistCardRecipient = tryFindCardByCode transactionInput.Code
        if isExistCardSender.IsNone then return Error { Message = "Not found card" }
        else if isExistCardRecipient.IsNone then return Error { Message = "Not found card with this code" }
        else if transactionInput.Amount < 1 then return Error { Message = "Not available transaction" } else
        let cardSender = isExistCardSender.Value
        let cardRecipient = isExistCardRecipient.Value
        if cardSender.Code = cardRecipient.Code then return Error { Message = "Not available transaction" }
        else if cardRecipient.Status = Deactivate then return Error { Message = "Not available transaction"} else 
        let! transactionsByCardSender = getTransactionsByCardId cardSender.Id
        let cardSenderWithUpdateTransactions = { cardSender with Transactions = transactionsByCardSender }
        let isSuccessTransaction = processPayment cardSenderWithUpdateTransactions transactionInput.Amount cardRecipient.Id transactionInput.Message
        match isSuccessTransaction with
        | Error error -> return Error error
        | Ok updateCard ->
            let newTransaction = Seq.last updateCard.Transactions
            do! saveTransaction newTransaction |> Async.Ignore
            do! updateCardBalanceById updateCard.Id updateCard.Balance |> Async.Ignore
            do! updateCardBalanceById cardRecipient.Id (cardRecipient.Balance + transactionInput.Amount) |> Async.Ignore
            return Ok newTransaction
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private getUserTransactions userId cardId = async {
    try
        let! cards = getCards userId
        let isExistCard = cards |> Seq.tryFind (fun v -> v.Id = cardId)
        match isExistCard with
        | None -> return Error { Message = "It's not your card" }
        | Some card ->
            let! transactionsByCard = getTransactionsByCardId card.Id
            let! transactionsToUserId = getTransactionsToCardId cardId
            let allTransactions = Seq.concat [transactionsByCard; transactionsToUserId]
            return Ok allTransactions
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private blockCard userId cardId = async {
    try
        let! cards = getCards userId
        let isExistCard = cards |> Seq.tryFind (fun v -> v.Id = cardId)
        match isExistCard with
        | None -> return Error { Message = "Not found card" }
        | Some card ->
            do! updateStatusCard cardId Deactivate |> Async.Ignore
            return { card with Status = Deactivate } |> Ok
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getCardsStoreImplementation ctx: ICardsStore =
    let userId = getUserIdFromHttpContext ctx
    {
        GetCards =  getUserCards userId
        CreateCard =  createCard userId
        CreateTransaction = createTransaction userId
        GetTransactions = getUserTransactions userId
        BlockCard = blockCard userId 
    }