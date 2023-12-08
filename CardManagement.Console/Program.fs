open System
open CardManagement.Data.UsersRepository
open CardManagement.Data.CardsRepository
open CardManagement.Infrastructure.DomainModels
open CardManagement.Infrastructure.CardActions

[<EntryPoint>]
let main _ =
    let id = Guid.NewGuid()
    let mockUser : User = {
        Id = Guid.Parse "d88152a7-e8fc-4814-a062-c7d9aacf968a"
        Name = "John"
        Surname = "Doe"
        Patronymic = "Smith"
        Age = 30
        Salary = 50000
        Email = "john.doe@exampl.com"
        Cards = [] 
    }
    let user = tryFindUserById "johndoe@exampl.com" |> Async.AwaitTask |> Async.RunSynchronously
    printfn "%A" user
    0 