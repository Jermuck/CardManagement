namespace CardManagement.Data

module DatabaseContext =
    open System.ComponentModel.DataAnnotations
    open Microsoft.EntityFrameworkCore
    open CardManagement.Data.DatabaseConfiguration
    
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
            options.UseNpgsql("host=localhost; port=5432; database=t; username=postgres; password=postgres") |> ignore