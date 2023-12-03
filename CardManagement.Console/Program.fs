open System
open CardManagement.Data.UsersRepository
open CardManagement.Infrastructure.DomainModels
open CardManagement.Infrastructure.CardActions

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
       Cards = []
    }
    let card = buildCard mockUser Basic 100
    let t = createUser {mockUser with Cards = [card] }
    0 