namespace CardManagement.API

open CardManagement.Infrastructure.DomainModels

module UserStore =
    open CardManagement.Infrastructure.InputTypes
    open CardManagement.Data.UsersRepository
    open CardManagement.API.SharedTypes
    open CardManagement.Infrastructure.UserActions
    open CardManagement.API.JWT
    
    let private register (inputUser: InputUser): Async<Result<RegistrationResponse>> = async {
        let! isExistUser = tryFindUserByEmail inputUser.Email |> Async.AwaitTask
        if isExistUser.IsSome then return Error { Message = "User with this email already exist" }
        else
            let user = buildUser inputUser
            let token = userToToken user
            saveUser user |> Async.AwaitTask |> ignore
            return Ok (user, token)
    }

    
    let usersStoreImplementation = {
        Register = register 
    }