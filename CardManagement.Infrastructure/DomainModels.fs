namespace CardManagement.Infrastructure

open System

module DomainModels =
    
    type TypeOfActivation =
        | Activate
        | Deactivate
        
    type TypeOfCard =
        | Basic
        | Priority
        | Credit
        
    type Transaction = {
        Id: string
        Card: Card
        CreateDate: DateTime
        Sum: int
        To: string
    }
        
    and Card = {
        Id: string
        Code: int
        CVV: int
        UserId: string
        TypeCard: TypeOfCard
        Balance: int
        Transactions: List<Transaction> option
        LifeTime: DateTime
        Status: TypeOfActivation
    }

    type User = {
        Id: string
        Name: string
        Surname: string
        Patronymic: string
        Age: int
        Salary: int
        Email: string
        Cards: List<Card[]> option
    }
