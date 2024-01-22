module CardManagement.Database.CardsRepository

open SqlHydra.Query
open CardManagement.Database
open ``public``
open DatabaseContext
open Mappers

let saveCard card =
    let dbCard = mapCardToDB card
    insertAsync (Create openContext) {
        into cards
        entity dbCard
    }

let getCards userId = async {
    let! cards = selectAsync HydraReader.Read (Create openContext) {
        for card in cards do
        where (card.user_id = userId)
        select card
    }
    return cards |> Seq.map mapDBCardToDomain
}

let getCardsWithTransactions userId = async {
    let! cards = selectAsync HydraReader.Read (Create openContext) {
        for card in cards do
        join transaction in transactions on (card.id = transaction.card_id)
        where (card.user_id = userId)
        select (card, transaction)
    }
    return cards
}

let tryFindCardByCode code = async {
    let! cards = selectAsync HydraReader.Read (Create openContext) {
        for card in cards do
        where (card.code = code)
        select card
    }
    match Seq.isEmpty cards with
    | true -> return None
    | false -> return cards |> Seq.item 0 |> mapDBCardToDomain |> Some  
}

let updateCardBalanceById cardId balance =
    updateAsync (Create openContext) {
        for card in cards do
        set card.balance balance
        where (card.id = cardId)
    }
    
let updateStatusCard cardId status =
    let dbStatus = mapStatusOfCardToDB status
    updateAsync (Create openContext) {
        for card in cards do
        set card.status dbStatus
        where (card.id = cardId)
    }