module CardManagement.Infrastructure.CardActions

open CardManagement.Shared.Types
open System
open Microsoft.FSharp.Core

let deactivate (card: Card) =
    match card.Status with
    | Activate -> { card with Status = Deactivate }
    | Deactivate -> card

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
    
let getIsAvailableCard (user: User) =
    if user.Salary >= 100_000 then seq [ Priority; Basic ]
    else [ Basic ]
    
let buildCard (user: User) (typeCard: TypeOfCard) (balance: int) =
    let random = Random()
    let code = Math.Abs(random.Next(100_000_000, 999_999_999) * 1000)
    let CVV = random.Next(100, 999)
    let id = Guid.NewGuid()
    let lifeTime = DateTime.Now.AddYears(4)
    let card = {
        Id = id
        Code = code
        CVV = CVV
        UserId = user.Id
        TypeCard = typeCard
        Balance = balance
        Transactions = []
        LifeTime = lifeTime
        Status = Activate 
    }
    card
    
let private makeTransaction (card: Card) (sumTransaction: int) (toId: Guid) =
    let newTransaction = {
        Id = toId
        CardId = card.Id
        Sum = sumTransaction
        CreateDate = DateTime.Now
        ToUserId = toId 
    }
    { card with Transactions = [newTransaction] |> Seq.append card.Transactions }

let private getTransactionsSum transactions =
    transactions
    |> Seq.filter (fun e -> e.CreateDate.Day = DateTime.Now.Day)
    |> Seq.map (_.Sum)
    |> Seq.sum

let private getPossiblyTransaction (card: Card) (amount: int) =
    let transactionsSum = getTransactionsSum card.Transactions
    match card.TypeCard with
    | Priority -> transactionsSum + amount <= 300_000
    | Basic -> transactionsSum + amount <= 100_000
    
let processPayment (card: Card) (amount: int) (toUserId: Guid) =
    match card.Status with
    | Deactivate  -> Error "Card deactivate"
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