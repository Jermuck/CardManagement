module CardManagement.Server.CardsStore

open CardManagement.Shared.Core
open CardManagement.Shared.Types
open CardManagement.Data.CardsRepository
open CardManagement.Data.UsersRepository
open CardManagement.Server.RemotingUtils
open CardManagement.Infrastructure.CardActions
open System

let private getCards (userId: Guid) () = async {
    try
        let! cards = getCards userId |> Async.AwaitTask
        return Ok cards
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private create (userId: Guid) (typeCard: TypeOfCard) = async {
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
                printfn "%A" newCard.Code
                newCard |> saveCard |> Async.AwaitTask |> ignore
                return Ok newCard;
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getCardsStoreImplementation ctx: ICardsStore =
    let userId = getUserIdFromHttpContext ctx
    {
        Get = getCards userId
        Create = create userId 
    }

