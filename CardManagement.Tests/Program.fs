open CardManagement.Data.UsersRepository

[<EntryPoint>]
let main _ =
    let user = tryFindUserByEmail "johndoe@exampl.com" |> Async.AwaitTask |> Async.RunSynchronously
    printfn "%A" user
    0 
