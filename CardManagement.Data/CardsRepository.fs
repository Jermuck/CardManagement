namespace CardManagement.Data

module CardsRepository =
    open CardManagement.Data.DatabaseContext
    open CardManagement.Infrastructure.DomainModels
    open System
    open CardManagement.Data.``public``
    open SqlHydra.Query
    open CardManagement.Data.Mappers
    
    let saveCard (card: Card) =
        let dbCard = convertCardToDB card
        insertTask (Create openContext) {
            into cards
            entity dbCard
        }
