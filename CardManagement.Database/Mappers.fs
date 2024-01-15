module CardManagement.Database.Mappers

open CardManagement.Database.``public``
open CardManagement.Shared.Types
open System

let private convertTimeToDateOnly (time: DateTime) =
   DateOnly(time.Year, time.Month, time.Hour)

let private convertTimeToDateTime (time: DateOnly) =
    DateTime(time.Year, time.Month, time.Day)

let private mapTypeOfCardToDB (typeCard: TypeOfCard) =
    match typeCard with
    | Basic -> typeofcard.basic
    | Priority -> typeofcard.priority

let private mapStatusOfCardToDB (status: TypeOfActivation) =
    match status with
    | Activate -> statusofcard.activate
    | Deactivate -> statusofcard.deactivate

let private mapTypeOfCardToDomain (typeCard: typeofcard) =
    match typeCard with
    | typeofcard.basic -> Basic
    | typeofcard.priority -> Priority
    | _ -> Basic

let private mapStatusOfCardToDomain (status: statusofcard) =
    match status with
    | statusofcard.activate -> Activate
    | statusofcard.deactivate -> Deactivate
    | _ -> Deactivate

let mapUserToDB (user: User) =
    {
        id = user.Id
        name = user.Name
        surname = user.Surname
        patronymic = user.Patronymic
        password = user.Password
        salary = user.Salary
        age = user.Age
        email = user.Email
    }

let mapDBUserToDomain (user: users) =
    {
        Id = user.id
        Name = user.name
        Surname = user.surname
        Patronymic = user.patronymic
        Password = user.password 
        Salary = user.salary
        Age = user.age
        Email = user.email
        Cards = [] 
    }

let mapCardToDB (card: Card) =
    let type_card = mapTypeOfCardToDB card.TypeCard
    let status = mapStatusOfCardToDB card.Status
    let lifeTime = convertTimeToDateOnly card.LifeTime
    {
            id = card.Id
            code = card.Code
            cvv = card.CVV
            user_id = card.UserId
            type_card = type_card
            balance = card.Balance
            life_time = lifeTime
            status = status
    }

let mapDBCardToDomain (card: cards) =
    let typeCard = mapTypeOfCardToDomain card.type_card
    let status = mapStatusOfCardToDomain card.status
    let time = convertTimeToDateTime card.life_time
    {
        Id = card.id
        Code = card.code
        CVV = card.cvv
        UserId = card.user_id
        TypeCard = typeCard
        Balance = card.balance
        LifeTime = time
        Status = status
        Transactions = [] 
    }
    
let mapTransactionToDB (transaction: Transaction) =
    let date = convertTimeToDateOnly transaction.CreateDate
    {
        id = transaction.Id
        sum = transaction.Sum
        card_id = transaction.CardId
        to_user_id = transaction.ToUserId
        create_date = date
        message = transaction.Message 
    }