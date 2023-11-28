namespace CardManagement.Data

open System
open CardManagement.Data.DatabaseContext

module CardsRepository =
    let databaseContext = new DatabaseContext()
    
    let findCardByUserId (id: Guid) =
        query {
            for card in databaseContext.cards do
                where (card.User.Id = id)
                select card
        } |> Seq.toList
