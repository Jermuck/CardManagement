module CardManagement.Infrastructure.CardActions

open System
open CardManagement.Shared
open Types

let private getCardExpired card =
    let now = DateTime.Now
    card.LifeTime > now
    
let private buildTransaction cardId sumTransaction toCardId message =
    {
        Id = Guid.NewGuid()
        CardId = cardId
        Sum = sumTransaction
        CreateDate = DateTime.Now
        ToCardId = toCardId
        Message = message 
    }
    
let private makeTransaction (card: Card) sumTransaction toCardId message =
    let newTransaction = buildTransaction card.Id sumTransaction toCardId message
    { card with Transactions = [newTransaction] |> Seq.append card.Transactions }

let private getTransactionsSum transactions =
    transactions
    |> Seq.filter (fun e -> e.CreateDate.Day = DateTime.Now.Day)
    |> Seq.map (_.Sum)
    |> Seq.sum

let private getPossiblyTransaction card amount =
    let transactionsSum = getTransactionsSum card.Transactions
    match card.TypeCard with
    | Priority -> transactionsSum + amount <= 300_000
    | Basic -> transactionsSum + amount <= 100_000

let private getValidBalance card sum =
    let remnant = card.Balance - sum
    if remnant >= 0 then Some remnant
    else None
    
let deactivate card =
    match card.Status with
    | Activate -> { card with Status = Deactivate }
    | Deactivate -> card

let activate card =
    match card.Status with
    | Deactivate -> {card with Status = Activate }
    | Activate -> card

let getIsAvailableCard (user: User) =
    if user.Salary >= 100_000 then seq [ Priority; Basic ]
    else [ Basic ]
    
let buildCode() =
   let random = Random()
   let code = int64(Math.Abs(random.Next(100_000_000, 999_999_999))).ToString()
   let endCode = Math.Abs(random.Next(1_000_000, 9_999_999)).ToString()
   int64(code + endCode)
   
let buildCard user typeCard balance =
    let random = Random()
    let code = buildCode()
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

let processPayment card amount toCardId message =
    match card.Status with
    | Deactivate -> Error { Message = "Card deactivate" }
    | Activate ->
        let isExpiredCard = getCardExpired card
        let isValidBalanceCard = getValidBalance card amount
        let isPossiblyTransactionCard = getPossiblyTransaction card amount
        if not isExpiredCard then Error { Message = "This card has expired" }
        else if not isPossiblyTransactionCard then Error { Message = "The limit on the card has been exceeded" }
        else
            match isValidBalanceCard with
            | Some currentBalance -> makeTransaction { card with Balance = currentBalance } amount toCardId message |> Ok
            | None -> Error { Message = "Insufficient funds on the card" }