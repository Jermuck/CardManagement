module CardManagement.Database.TransactionsRepository

open SqlHydra.Query
open CardManagement.Database
open DatabaseContext
open Mappers
open ``public``

let saveTransaction transaction =
    let mapToDBTransaction = mapTransactionToDB transaction
    insertAsync (Create openContext) {
        into transactions
        entity mapToDBTransaction
    }
    
let getTransactionsByCardId cardId = async {
    let! result = selectAsync HydraReader.Read (Create openContext) {
        for transaction in transactions do
        where (transaction.card_id = cardId)
        select transaction
    }
    return result |> Seq.map mapDBTransactionToDomain
}

let getTransactionsToCardId cardId = async {
    let! result = selectAsync HydraReader.Read (Create openContext) {
        for transaction in transactions do
        where (transaction.to_card_id = cardId)
        select transaction
    }
    return result |> Seq.map mapDBTransactionToDomain
} 
