module CardManagement.Database.TransactionsRepository

open CardManagement.Shared.Types
open CardManagement.Database.DatabaseContext
open SqlHydra.Query
open CardManagement.Database.Mappers
open CardManagement.Database.``public``

let saveTransaction (transaction: Transaction) =
    let mapToDBTransaction = mapTransactionToDB transaction
    insertTask (Create openContext) {
        into transactions
        entity mapToDBTransaction
    }