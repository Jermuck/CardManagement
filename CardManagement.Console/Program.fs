open CardManagement.Data.Repository

[<EntryPoint>]
let main _ =
    let t = test() |> Async.AwaitTask |> Async.RunSynchronously
    printfn "%A" t
    0 