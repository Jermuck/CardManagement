module CardManagement.Shared.Types

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
    
type Card = {
    Id: Guid
    Code: int64
    CVV: int
    UserId: Guid
    TypeCard: TypeOfCard
    Balance: int
    Transactions: Transaction seq
    LifeTime: DateTime
    Status: TypeOfActivation
}

type User = {
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

type InputUser = {
    Name: string
    Surname: string
    Patronymic: string
    Password: string
    Age: int
    Salary: int
    Email: string
}

type ResponseError = {
    Message: string
}

type ResponseResult<'a> =
    | Ok of 'a
    | Error of ResponseError
    
type RegistrationResponse = User * Token