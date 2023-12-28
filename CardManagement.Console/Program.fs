open System
open CardManagement.Data.UsersRepository
open CardManagement.Data.CardsRepository
open CardManagement.Infrastructure.DomainModels
open CardManagement.Infrastructure.CardActions

[<EntryPoint>]
let main _ =
    let user = tryFindUserByEmail "johndoe@exampl.com" |> Async.AwaitTask |> Async.RunSynchronously
    printfn "%A" user
    0 
