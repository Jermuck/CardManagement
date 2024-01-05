module CardManagement.Data.CardsRepository

open CardManagement.Data.DatabaseContext
open CardManagement.Shared.Types
open CardManagement.Data.``public``
open CardManagement.Data.Mappers
open SqlHydra.Query

let saveCard (card: Card) =
    let dbCard = convertCardToDB card
    insertTask (Create openContext) {
        into cards
        entity dbCard
    }
