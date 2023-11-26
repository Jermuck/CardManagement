namespace CardManagement.Data

open System
open EntityFrameworkCore.FSharp

module DatabaseContext =
    open Microsoft.EntityFrameworkCore
    open CardManagement.Infrastructure.DomainModels
    open CardManagement.Data.DatabaseConfiguration
    
    type DatabaseContext() =  
        inherit DbContext()
        
        [<DefaultValue>] val mutable users: DbSet<User>
        [<DefaultValue>] val mutable cards: DbSet<Card>
        [<DefaultValue>] val mutable transactions: DbSet<Transaction>
        
        member __.Users with get() = __.users and set v = __.users <- v
        member __.Cards with get() = __.cards and set v = __.cards <- v
        member __.Transactions with get() = __.transactions and set v = __.transactions <- v
        
        override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
            options.UseNpgsql(databaseConnectionString) |> ignore
        
        override __.OnModelCreating(modelBuilder: ModelBuilder) =
                
            modelBuilder.Entity<Card>()
                .Property(fun card -> card.TypeCard)
                .HasConversion(SingleCaseUnionConverter<int, TypeOfCard>()) |> ignore
            
            modelBuilder.Entity<Card>()
                .Property(fun card -> card.Status)
                .HasConversion(SingleCaseUnionConverter<int, TypeOfActivation>()) |> ignore

            