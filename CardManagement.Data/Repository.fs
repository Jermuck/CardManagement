namespace CardManagement.Data


module Repository =
    open SqlHydra.Query
    open CardManagement.Data.``public``
    open Npgsql
    
    let openContext() = 
        let compiler = SqlKata.Compilers.PostgresCompiler()
        let conn = new NpgsqlConnection("server=localhost; database=cardmanagement; username=root; password=12345")
        conn.Open() |> ignore
        new QueryContext(conn, compiler)
    
    let test() =
        insertTask (Create openContext) {
            for p in customers do
                entity {
                    customer_id = 4
                    customer_name = "dasda"
                }
        }