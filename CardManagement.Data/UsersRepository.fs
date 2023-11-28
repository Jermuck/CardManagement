namespace CardManagement.Data

open System

module UsersRepository =
    open CardManagement.Data.DatabaseContext
    open CardManagement.Data.Mappers
    open CardManagement.Infrastructure.DomainModels
    
    let private databaseContext = new DatabaseContext()
    
    let getUserById (id: Guid) =
        databaseContext.users.Find id
    
    let getIsUniqueByEmail (email: string) =
        let usersSeq = query {
            for user in databaseContext.users do
                where (user.Email = email)
                select user
        }
        usersSeq
        |> Seq.toList
        
    
    let createUser (user: User) =
        let mapUser = mapToDatabaseUser user mapToDatabaseCard
        let dbUser = databaseContext.users.Add mapUser
        databaseContext.SaveChanges() |> ignore
        dbUser
    
    let findUsers() =
        query {
            for user in databaseContext.users do
                select user
        } |> Seq.toList
    