namespace CardManagement.Data

open CardManagement.Data.DatabaseModels

module Mappers =
   open CardManagement.Data.DatabaseModels
   open CardManagement.Infrastructure.DomainModels
   
   let mapToDatabaseUser (user: User) (mapToDatabaseCard: Card -> DBCard): DBUser  =
        let cards =
            user.Cards
            |> List.map mapToDatabaseCard
            |> ResizeArray
        {
            Id = user.Id
            Age = user.Age
            Name = user.Name
            Patronymic = user.Patronymic 
            Salary = user.Salary
            Surname = user.Surname
            Email = user.Email
            Cards = cards
        }
   
   let mapToDatabaseTransaction (transaction: Transaction) (mapToDatabaseCard: Card -> DBCard): DBTransaction =
        let dateTime = transaction.CreateDate.ToUniversalTime()
        let card = mapToDatabaseCard transaction.Card
        {
            Id = transaction.Id
            Card = card
            CreateDate = dateTime
            Sum = transaction.Sum
            ToUserId = transaction.ToUserId
        }
 
   let rec mapToDatabaseCard (card: Card): DBCard =
        let user = mapToDatabaseUser card.User mapToDatabaseCard
        let dateTime = card.LifeTime.ToUniversalTime()
        let transactions =
            card.Transactions
            |> List.map (fun transaction -> mapToDatabaseTransaction transaction mapToDatabaseCard)
            |> ResizeArray
        {
            Id = card.Id
            Code = card.Code
            Status = card.Status
            TypeCard = card.TypeCard 
            CVV = card.CVV
            User = user
            Balance = card.Balance
            Transactions = transactions
            LifeTime = dateTime
        } 

  