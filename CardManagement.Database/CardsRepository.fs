module CardManagement.Data.CardsRepository

open System
open CardManagement.Data.DatabaseContext
open CardManagement.Shared.Types
open CardManagement.Data.``public``
open CardManagement.Data.Mappers
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