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
        let! cards = getCards userId |> Async.AwaitTask
        return Ok cards
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private createCard userId typeCard = async {
    try
        let! user = tryFindUserById userId |> Async.AwaitTask
        if user.IsNone then return Error { Message = "User with this id not found" }
        else
            let availableTypesCard = getIsAvailableCard user.Value
            let isExistTypeCard = availableTypesCard |> Seq.contains typeCard
            match isExistTypeCard with
            | false -> return Error { Message = "Sorry, We can't create this card for you" }
            | true ->
                let newCard = buildCard user.Value typeCard 1000
                newCard |> saveCard |> Async.AwaitTask |> ignore
                return Ok newCard;
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private createTransaction userId transactionInput = async {
    try
        let! isExistCardRecipient = tryFindCardByCode transactionInput.Code |> Async.AwaitTask
        let! cardsSender = getCards userId |> Async.AwaitTask
        let isExistCardSender = cardsSender |> Seq.tryFind (fun v -> v.Id = transactionInput.CardIdSender)
        if isExistCardRecipient.IsNone then return Error { Message = "Error card with this code not found" }
        else if isExistCardSender.IsNone then return Error { Message = "Server error" } else
        let recipientId = isExistCardRecipient.Value.UserId
        let isSuccessTransaction = processPayment isExistCardSender.Value transactionInput.Amount recipientId transactionInput.Message
        match isSuccessTransaction with
        | Error error -> return Error { Message = error.Message }
        | Ok cardWithUpdateTransactions ->
            let newTransactionBySender = cardWithUpdateTransactions.Transactions |> Seq.last
            newTransactionBySender |> saveTransaction |> Async.AwaitTask |> ignore
            return Ok newTransactionBySender
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}
    

let getCardsStoreImplementation ctx: ICardsStore =
    let userId = getUserIdFromHttpContext ctx
    {
        Get = getUserCards userId
        Create = createCard userId
        CreateTransaction = createTransaction userId 
    }

