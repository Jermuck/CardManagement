namespace CardManagement.Data

module DatabaseContext =
    open System.ComponentModel.DataAnnotations
    open EntityFrameworkCore.FSharp.Extensions
    open Microsoft.EntityFrameworkCore
    
    [<CLIMutable>]
    type Blog = {
        [<Key>] Id: int
        Url: string
    }

    type BloggingContext() =  
        inherit DbContext()
    
        [<DefaultValue>] val mutable blogs : DbSet<Blog>
        member this.Blogs with get() = this.blogs and set v = this.blogs <- v
        
        override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
            options.UseNpgsql("Server=localhost;Username=postgres;Password=postgres;Database=t") |> ignore