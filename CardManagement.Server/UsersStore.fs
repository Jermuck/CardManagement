namespace CardManagement.API

module UserStore =
    open CardManagement.Data.UsersRepository
    open CardManagement.Shared.Types
    open CardManagement.Shared.Core
    open CardManagement.Infrastructure.UserActions
    open CardManagement.API.JWT
    open CardManagement.API.Password
    
    let private register inputUser = async {
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
    
    let privateStoreImplementation: IPrivateStore = {
        Get = getMyProfile
    }
    
    let usersStoreImplementation: IUsersStore = {
        Register = fun _ -> async {
            return Error { Message = "User with this email already exist" }
        }  
    }
