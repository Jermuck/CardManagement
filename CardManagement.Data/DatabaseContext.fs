namespace CardManagement.Data

open CardManagement.Data.``public``
open CardManagement.Data.``public``
open CardManagement.Infrastructure.DomainModels


module DatabaseContext =
    open SqlHydra.Query
    open Npgsql
    open CardManagement.Data.Settings
    
    let openContext() = 
        let compiler = SqlKata.Compilers.PostgresCompiler()
        let dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString)
        dataSourceBuilder.MapEnum<typeofcard>() |> ignore
        dataSourceBuilder.MapEnum<statusofcard>() |> ignore
        let build = dataSourceBuilder.Build()
        let connection = build.OpenConnection()
        new QueryContext(connection, compiler)
    