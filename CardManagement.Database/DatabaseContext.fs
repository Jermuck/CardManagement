module CardManagement.Database.DatabaseContext 

open SqlHydra.Query
open Npgsql
open CardManagement.Database.Settings
open CardManagement.Database.``public``

let openContext() = 
    let compiler = SqlKata.Compilers.PostgresCompiler()
    let dataSourceBuilder = NpgsqlDataSourceBuilder(connectionString)
    dataSourceBuilder.MapEnum<typeofcard>() |> ignore
    dataSourceBuilder.MapEnum<statusofcard>() |> ignore
    let connection = dataSourceBuilder.Build().OpenConnection()
    new QueryContext(connection, compiler)
