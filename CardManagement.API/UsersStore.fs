namespace CardManagement.API

module UserStore =
    open CardManagement.Infrastructure.InputTypes
    open CardManagement.Data.UsersRepository
    open CardManagement.API.SharedTypes
    open CardManagement.Infrastructure.UserActions
    open CardManagement.API.JWT
    open CardManagement.API.Password
    
    let private register (inputUser: InputUser): Async<Result<RegistrationResponse>> = async {
        let! isExistUser = tryFindUserByEmail inputUser.Email |> Async.AwaitTask
        if isExistUser.IsSome then return Error { Message = "User with this email already exist" }
        else
            let user = buildUser inputUser
            let hashPassword = createHash user.Password
            let userWithUpdateHashPassword = { user with Password = hashPassword }
            let token = userToToken userWithUpdateHashPassword
            saveUser userWithUpdateHashPassword |> Async.AwaitTask |> ignore
            return Ok (user, token)
    }
    
    let getMyProfile() = async {
        return "hello"
    }
    
    let privateStoreImplementation = {
        Get = getMyProfile
    }
    
    let usersStoreImplementation = {
        Register = register 
    }
