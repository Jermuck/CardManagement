namespace CardManagement.Data

open System
open CardManagement.Data.``public``

module CardsRepository =
    open CardManagement.Data.DatabaseContext
    open CardManagement.Infrastructure.DomainModels
    open SqlHydra.Query
    
    let private convertStatus (status: TypeOfActivation): statusofcard =
        match status with
        | Activate -> statusofcard.activate
        | Deactivate -> statusofcard.deactivate
        
    
    let createCard (card: Card) =
        insertTask (Create openContext) {
            into cards
            entity {
                id = card.Id
                code = card.Code
                cvv = card.CVV
                user_id = card.UserId
                type_card = typeofcard.basic
                balance = card.Balance
                life_time = DateOnly(card.LifeTime.Year, card.LifeTime.Month, card.LifeTime.Hour)
                status = statusofcard.activate
            }
        }

