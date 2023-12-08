namespace CardManagement.API

open System
open CardManagement.API.SharedTypes

module UsersController =
    open CardManagement.Data.UsersRepository
    open CardManagement.Infrastructure.DomainModels
    
    let register (user: UserDTO): Async<Result<User>> = async {
        let! isSomeUser = tryFindUserByEmail user.Email |> Async.AwaitTask
        if isSomeUser.IsSome then return Error { message = "User with this email already exist" }
        else
            let user = {
                Id = Guid.NewGuid()
                Name = "John"
                Surname = "Doe"
                Patronymic = "Smith"
                Age = 30
                Salary = 50000
                Email = "john.doe@exampl.com"
                Cards = [] 
            }
            saveUser user |> Async.AwaitTask |> ignore
            return Ok user
    }
        

