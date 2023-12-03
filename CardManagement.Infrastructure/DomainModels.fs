namespace CardManagement.Infrastructure

module DomainModels =
    open System
    type TypeOfActivation =
        | Activate
        | Deactivate 
        
    type TypeOfCard =
        | Basic
        | Priority
        
    [<CLIMutable>]    
    type Transaction = {
        Id: Guid
        Card: Card
        CreateDate: DateTime
        Sum: int
        ToUserId: Guid
    }
    
    and [<CLIMutable>] Card = {
        Id: Guid
        Code: int32
        CVV: int
        User: User
        TypeCard: TypeOfCard
        Balance: int
        Transactions: Transaction seq
        LifeTime: DateTime
        Status: TypeOfActivation
    }
    
    and [<CLIMutable>] User = {
        Id: Guid
        Name: string
        Surname: string
        Patronymic: string
        Age: int
        Salary: int
        Email: string
        Cards: Card seq
    }
