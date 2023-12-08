namespace CardManagement.Data

module DatabaseContext =
    open SqlHydra.Query
    open Npgsql
    open CardManagement.Data.Settings
    open CardManagement.Data.``public``
    
    let openContext() = 
        let compiler = SqlKata.Compilers.PostgresCompiler()
        let dataSourceBuilder = NpgsqlDataSourceBuilder(connectionString)
        dataSourceBuilder.MapEnum<typeofcard>() |> ignore
        dataSourceBuilder.MapEnum<statusofcard>() |> ignore
        let build = dataSourceBuilder.Build()
        let connection = build.OpenConnection()
        new QueryContext(connection, compiler)
    