open System
open CardManagement.Infrastructure.DomainModels
open CardManagement.Infrastructure.CardActions

[<EntryPoint>]
let main _ =
    let card = {
        Id = Guid.NewGuid()
        Code = 444
        CVV = 444
        UserId = Guid.NewGuid()
        TypeCard = Priority
        Balance = 100
        Transactions = None
        LifeTime = DateTime.Now
        Status = Activate
    }
    let newId = Guid.NewGuid()
    let updateCard = processPayment card 10 newId
    match updateCard with
    | Error msg -> printfn "%A" msg
    | Ok card -> printfn "%A" card
    0