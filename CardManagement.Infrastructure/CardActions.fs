namespace CardManagement.Infrastructure

open System

module CardActions =
    open System
    open CardManagement.Infrastructure.DomainModels
    
    let deactivate (card: Card) =
        match card.Status with
        | Activate -> {card with Status = Deactivate }
        | Deactivate -> card
    
    let private getTransactionsSum transactions =
        transactions
        |> List.filter (fun e -> e.CreateDate.Day = DateTime.Now.Day)
        |> List.map (fun e -> e.Sum)
        |> List.sum
    
    let activate (card: Card) =
        match card.Status with
        | Deactivate -> {card with Status = Activate }
        | Activate -> card
    
    let private getCardExpired (card: Card) =
        let now = DateTime.Now
        card.LifeTime > now
    
    let private getValidBalance (card: Card) (sum: int) =
        let remnant = card.Balance - sum
        if remnant >= 0 then Some remnant
        else None
    
    let private makeTransaction (card: Card) (sumTransaction: int) (toId: Guid) =
        let newTransaction = {
            Id = toId
            CardId = card.Id
            Sum = sumTransaction
            CreateDate = DateTime.Now
            ToUserId = toId 
        }
        match card.Transactions with
        | None -> {card with Transactions = Some [newTransaction] }
        | Some transactions -> {card with Transactions = Some ([newTransaction] |> List.append transactions)}
    
    let private getPossiblyTransaction (card: Card) (amount: int) =
        match card.Transactions with
        | Some transactions -> 
            let transactionsSum = getTransactionsSum transactions
            match card.TypeCard with
            | Priority -> transactionsSum + amount <= 300_000
            | Basic -> transactionsSum + amount <= 100_000
        | None -> true
    
    let processPayment (card: Card) (amount: int) (toUserId: Guid) =
        match card.Status with
        | Deactivate -> Error "Card deactivate"
        | Activate ->
            let isExpiredCard = getCardExpired card
            let isValidBalanceCard = getValidBalance card amount
            let isPossiblyTransactionCard = getPossiblyTransaction card amount
            if not isExpiredCard then Error "This card has expired"
            else if not isPossiblyTransactionCard then Error "The limit on the card has been exceeded"
            else
                match isValidBalanceCard with
                | Some currentBalance -> makeTransaction {card with Balance = currentBalance } amount toUserId |> Ok
                | None -> Error "Insufficient funds on the card"