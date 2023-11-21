open CardManagement.Data

[<EntryPoint>]
let main _ =
    let stringOption = ConnectionOptions.connection
    printfn "%A" stringOption
    0