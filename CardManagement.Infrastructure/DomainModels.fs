namespace CardManagement.Infrastructure

module DomainModels =
    open System
    
    type TypeOfActivation =
        | Activate
        | Deactivate 
        
    type TypeOfCard =
        | Basic
        | Priority
        
    type Transaction = {
        Id: Guid
        CardId: Guid
        CreateDate: DateTime
        Sum: int
        ToUserId: Guid
    }
        
    and Card = {
        Id: Guid
        Code: int32
        CVV: int
        UserId: Guid
        TypeCard: TypeOfCard
        Balance: int
        Transactions: Transaction seq
        LifeTime: DateTime
        Status: TypeOfActivation
    }

    and User = {
        Id: Guid
        Name: string
        Surname: string
        Patronymic: string
        Password: string
        Age: int
        Salary: int
        Email: string
        Cards: Card seq
    }
    
    type Token = {
        Token: string
    }
    
