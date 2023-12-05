namespace CardManagement.Data

module Repository = 

    let openContext() = 

        let compiler = SqlKata.Compilers.SqlServerCompiler()
        let conn = new SqlConnection("Replace with your connection string")
        conn.Open()
        new QueryContext(conn, compiler)