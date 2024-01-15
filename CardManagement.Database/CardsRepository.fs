module CardManagement.Database.CardsRepository

open System
open CardManagement.Database.DatabaseContext
open CardManagement.Shared.Types
open CardManagement.Database.``public``
open CardManagement.Database.Mappers
open SqlHydra.Query

let saveCard (card: Card) =
    let dbCard = mapCardToDB card
    insertTask (Create openContext) {
        into cards
        entity dbCard
    }

let getCards (userId: Guid) = task {
    let! cards = selectTask HydraReader.Read (Create openContext) {
        for card in cards do
        where (card.user_id = userId)
        select card
    }
    return cards |> Seq.map mapDBCardToDomain
}

let tryFindCardByCode (code: int64) = task {
    let! cards = selectTask HydraReader.Read (Create openContext) {
        for card in cards do
        where (card.code = code)
        select card
    }
    match Seq.isEmpty cards with
    | true -> return None
    | false -> return cards |> Seq.item 0 |> mapDBCardToDomain |> Some  
}