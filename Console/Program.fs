open CardManagement.Infrastructure.DomainModels

[<EntryPoint>]
let main _ =
    let user = {
        Id = 1
        Name = ""
        Surname = "" 
    }
    printfn "%A" user
    0