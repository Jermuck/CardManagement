module CardManagement.Infrastructure.UserActions

open CardManagement.Shared.Types
open System

let buildUser (inputUser: InputUser): User =
    {
        Id = Guid.NewGuid()
        Name = inputUser.Name
        Surname = inputUser.Surname
        Patronymic = inputUser.Patronymic
        Age = inputUser.Age
        Password = inputUser.Password 
        Salary = inputUser.Salary
        Email = inputUser.Email
        Cards = [] 
    }
