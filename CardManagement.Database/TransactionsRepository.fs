module CardManagement.Database.TransactionsRepository

open System
open CardManagement.Shared.Types
open CardManagement.Database.DatabaseContext
open SqlHydra.Query
open CardManagement.Database.Mappers
open CardManagement.Database.``public``

let saveTransaction (transaction: Transaction) =
    let mapToDBTransaction = mapTransactionToDB transaction
    insertAsync (Create openContext) {
        into transactions
        entity mapToDBTransaction
    }
    
let getTransactionsByCardId (cardId: Guid) = async {
    let! result = selectAsync HydraReader.Read (Create openContext) {
        for transaction in transactions do
        where (transaction.card_id = cardId)
        select transaction
    }
    return result |> Seq.map mapDBTransactionToDomain
}

let getTransactionsToCardId (cardId: Guid) = async {
    let! result = selectAsync HydraReader.Read (Create openContext) {
        for transaction in transactions do
        where (transaction.to_card_id = cardId)
        select transaction
    }
    return result |> Seq.map mapDBTransactionToDomain
} 
