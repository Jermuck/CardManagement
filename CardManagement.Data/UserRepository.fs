namespace CardManagement.Data

module UserRepository =
    open CardManagement.Data.DatabaseContext
    open CardManagement.Data.Mappers
    open CardManagement.Infrastructure.DomainModels
    
    let private databaseContext = new DatabaseContext()
    
    let createUser (user: User) =
        let mapUser = mapToDatabaseUser user mapToDatabaseCard
        printfn "%A" mapUser
        let dbUser = databaseContext.users.Add mapUser
        databaseContext.SaveChanges() |> ignore
        dbUser
        