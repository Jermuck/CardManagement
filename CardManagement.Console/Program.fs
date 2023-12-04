open System
open System.Threading.Tasks
open CardManagement.Infrastructure.DomainModels
open CardManagement.Infrastructure.CardActions
open CardManagement.Data.DatabaseConfiguration
[<EntryPoint>]
let main _ =
    let mockUser: User = {
       Id = Guid.NewGuid()
       Name = "John"
       Surname = "Doe"
       Patronymic = "Smith"
       Age = 30
       Salary = 50000
       Email = "john.doe@example.com"
    }
    let card = buildCard mockUser Basic 100
    let users = selectUsers() |> Task.WaitAll
    printfn "%A" users
    0 