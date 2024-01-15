module CardManagement.Server.ProfileStore

open CardManagement.Shared.Types
open CardManagement.Database.UsersRepository
open CardManagement.Shared.Core
open CardManagement.Server.RemotingUtils

let private getMyProfile userId () = async {
    try
        let! isExistUser = tryFindUserById userId |> Async.AwaitTask
        match isExistUser with
        | Some user -> return Ok user
        | None -> return Error { Message = "User not found" }
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getProfileStoreImplementation ctx: IProfileStore =
    let userId = getUserIdFromHttpContext ctx
    {
        GetMyProfile = getMyProfile userId
    }

 
