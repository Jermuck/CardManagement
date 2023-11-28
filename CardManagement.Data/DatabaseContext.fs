namespace CardManagement.Data

open CardManagement.Data.DatabaseModels

module DatabaseContext =
    open Microsoft.EntityFrameworkCore
    open CardManagement.Data.DatabaseConfiguration
    open EntityFrameworkCore.FSharp.Extensions
    
    type DatabaseContext() =  
        inherit DbContext()
        
        [<DefaultValue>] val mutable users: DbSet<DBUser>
        [<DefaultValue>] val mutable cards: DbSet<DBCard>
        [<DefaultValue>] val mutable transactions: DbSet<DBTransaction>
        
        member __.Users with get() = __.users and set v = __.users <- v
        member __.Cards with get() = __.cards and set v = __.cards <- v
        member __.Transactions with get() = __.transactions and set v = __.transactions <- v
        
        override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
            options.UseNpgsql(databaseConnectionString)
                .UseFSharpTypes() |> ignore
        