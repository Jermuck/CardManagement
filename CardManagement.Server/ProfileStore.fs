module CardManagement.Server.ProfileStore

open CardManagement.Shared
open CardManagement.Database
open CardManagement.Server
open Core
open RemotingUtils
open UsersRepository
open Types

let private checkChanges (inputUser: InputUser) (userFromDb: User) =
    inputUser.Name = userFromDb.Name
    && inputUser.Patronymic = userFromDb.Patronymic
    && inputUser.Surname = userFromDb.Surname
    && inputUser.Salary = userFromDb.Salary
    && inputUser.Age = userFromDb.Age
    && inputUser.Email = userFromDb.Email

let private getMyProfile userId () = async {
    try
        let! isExistUser = tryFindUserById userId
        match isExistUser with
        | Some user -> return Ok user
        | None -> return Error { Message = "User not found" }
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let private updateMyProfile userId inputUser = async {
    try
        let! isExistUser = tryFindUserById userId
        match isExistUser with
        | None -> return Error { Message = "User not found" }
        | Some user ->
            let isError = checkChanges inputUser user
            if isError then return Error { Message = "Not found changes" } else
            match user.Email = inputUser.Email with
            | true ->
                do! updateUser inputUser userId |> Async.Ignore
                return Ok "Your profile has been updated successfully"
            | false ->
                let! isFindByEmail = tryFindUserByEmail inputUser.Email
                match isFindByEmail with
                | Some _ -> return Error { Message = "This email already exist" }
                | None ->
                    do! updateUser inputUser userId |> Async.Ignore
                    return Ok "Your profile has been updated successfully"
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getProfileStoreImplementation ctx: IProfileStore =
    let userId = getUserIdFromHttpContext ctx
    {
        GetMyProfile = getMyProfile userId
        UpdateProfile = updateMyProfile userId 
    }

 
