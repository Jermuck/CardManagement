module CardManagement.Database.TransactionsRepository

open System
open CardManagement.Shared.Types
open CardManagement.Database.DatabaseContext
open SqlHydra.Query
open CardManagement.Database.Mappers
open CardManagement.Database.``public``

let saveTransaction (transaction: Transaction) =
    let mapToDBTransaction = mapTransactionToDB transaction
    insertAsync sqlHydraContext {
        into transactions
        entity mapToDBTransaction
    }
    
let getTransactionsByCardId (cardId: Guid) = async {
    let! result = selectAsync HydraReader.Read sqlHydraContext {
        for transaction in transactions do
        where (transaction.card_id = cardId)
        select transaction
    }
    return result |> Seq.map mapDBTransactionToDomain
}

let getTransactionsToUserId (userId: Guid) notEqualCardId = async {
    let! result = selectAsync HydraReader.Read sqlHydraContext {
        for transaction in transactions do
        where (transaction.to_user_id = userId && transaction.card_id <> notEqualCardId)
        select transaction
    }
    return result |> Seq.map mapDBTransactionToDomain
} 
