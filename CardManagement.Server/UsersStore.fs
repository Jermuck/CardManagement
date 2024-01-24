module CardManagement.Server.UserStore

open CardManagement.Infrastructure
open CardManagement.Server
open CardManagement.Shared
open CardManagement.Database
open UsersRepository
open UserActions
open JWT
open Password
open Core
open Types

let private register inputUser = async {
    try
        let! isExistUser = tryFindUserByEmail inputUser.Email
        if isExistUser.IsSome then return Error { Message = "User with this email already exist" } else
        let user = buildUser inputUser
        let hashPassword = createHash user.Password
        let userWithUpdateHashPassword = { user with Password = hashPassword }
        let token = userToToken userWithUpdateHashPassword
        do! saveUser userWithUpdateHashPassword |> Async.AwaitTask |> Async.Ignore
        return Ok (user, token)
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private login email password = async {
    try
        let! isExistUser = tryFindUserByEmail email
        if isExistUser.IsNone then return Error { Message = $"User with %s{email} not found" } else
        let user = isExistUser.Value
        let isValidPassword = verifyPassword user.Password password
        match isValidPassword with
        | false -> return Error { Message = "Not valid password" }
        | true ->
            let token = userToToken user
            return Ok (user, token)
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let usersStoreImplementation: IUsersStore = {
    Register = register
    Login = login
}
