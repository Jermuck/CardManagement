namespace CardManagement.Data

module Configuration =
    open Npgsql.FSharp
    let private port = 5432
    let private username = "root"
    let private password = "root"
    let private database = "CardManagement"
    let private host = "localhost"
    let connection =
        Sql.host host
        |> Sql.database (database.ToLower())
        |> Sql.username username
        |> Sql.password password
        |> Sql.port port
        |> Sql.formatConnectionString