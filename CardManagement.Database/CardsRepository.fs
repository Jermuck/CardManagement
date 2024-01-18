module CardManagement.Database.CardsRepository

open System
open CardManagement.Database
open CardManagement.Database.``public``
open CardManagement.Database.DatabaseContext
open CardManagement.Shared.Types
open CardManagement.Database.Mappers
open SqlHydra.Query

let saveCard (card: Card) =
    let dbCard = mapCardToDB card
    insertAsync (Create openContext) {
        into cards
        entity dbCard
    }

let getCards (userId: Guid) = async {
    let! cards = selectAsync HydraReader.Read (Create openContext) {
        for card in cards do
        where (card.user_id = userId)
        select card
    }
    return cards |> Seq.map mapDBCardToDomain
}

let tryFindCardByCode (code: int64) = async {
    let! cards = selectAsync HydraReader.Read (Create openContext) {
        for card in cards do
        where (card.code = code)
        select card
    }
    match Seq.isEmpty cards with
    | true -> return None
    | false -> return cards |> Seq.item 0 |> mapDBCardToDomain |> Some  
}

let updateCardBalanceById (cardId: Guid) (balance: int) =
    updateAsync (Create openContext) {
        for card in cards do
        set card.balance balance
        where (card.id = cardId)
    }