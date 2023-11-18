namespace CardManagement.Infrastructure

module CardActions =
    open System
    open CardManagement.Infrastructure.DomainModels
    
    let private isPriorityCard salary =
        salary >= 300_000
    
    let deactivate (card: Card) =
        match card.Status with
        | Activate -> {card with Status = Deactivate }
        | Deactivate -> card
    
    let activate (card: Card) =
        match card.Status with
        | Deactivate -> {card with Status = Activate }
        | Activate -> card
    
    let private isCardExpired (card: Card) =
        let now = DateTime.Now
        card.LifeTime > now
    
    let private isValidBalance (card: Card) (sum: int) =
        let remnant = card.Balance - sum
        if remnant >= 0 then Some remnant
        else None
    
    let private makeTransaction (card: Card) (sumTransaction: int) (toId: Guid) =
        let newTransaction = {
            Id = Guid.NewGuid()
            CardId = card.Id
            Sum = sumTransaction
            CreateDate = DateTime.Now
            ToUserId = toId 
        }
        match card.Transactions with
        | None -> {card with Transactions = Some [newTransaction] }
        | Some transactions -> {card with Transactions = Some ([newTransaction] |> List.append transactions)}
    
    let processPayment (card: Card) (amount: int) (toUserId: Guid) =
        match card.Status with
        | Deactivate -> Error "Card deactivate"
        | Activate ->
            let isExpired = isCardExpired card
            let isBalance = isValidBalance card amount
            if isExpired then Error "This card has expired"
            else
                match isBalance with
                | Some currentBalance -> makeTransaction card currentBalance toUserId |> Ok
                | None -> Error "Insufficient funds on the card"