namespace CardManagement.API

open System
open CardManagement.API.SharedTypes
open CardManagement.Infrastructure.InputTypes

module UsersController =
    open CardManagement.Data.UsersRepository
    open CardManagement.Infrastructure.DomainModels
    open CardManagement.Infrastructure.UserActions
    
    let register (inputUser: InputUser): Async<Result<User>> = async {
        let! isSomeUser = tryFindUserByEmail inputUser.Email |> Async.AwaitTask
        if isSomeUser.IsSome then return Error { message = "User with this email already exist" }
        else
            let user = buildUser inputUser 
            saveUser user |> Async.AwaitTask |> ignore
            return Ok user
    }
        

