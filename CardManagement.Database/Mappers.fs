module CardManagement.Data.Mappers

open CardManagement.Data.``public``
open CardManagement.Shared.Types
open System

let private convertTimeToDateOnly (time: DateTime) =
   DateOnly(time.Year, time.Month, time.Hour) 

let private convertTypeOfCard (typeCard: TypeOfCard) =
    match typeCard with
    | Basic -> typeofcard.basic
    | Priority -> typeofcard.priority

let private convertStatusOfCard (status: TypeOfActivation) =
    match status with
    | Activate -> statusofcard.activate
    | Deactivate -> statusofcard.deactivate

let convertUserToDB (user: User) =
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

let convertDBUserToDomain (user: users) =
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

let convertCardToDB (card: Card) =
    let type_card = convertTypeOfCard card.TypeCard
    let status = convertStatusOfCard card.Status
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
    
let convertTransactionToDB (transaction: Transaction) =
    let date = convertTimeToDateOnly transaction.CreateDate
    {
        id = transaction.Id
        sum = transaction.Sum
        card_id = transaction.CardId
        to_user_id = transaction.ToUserId
        create_date = date
    }