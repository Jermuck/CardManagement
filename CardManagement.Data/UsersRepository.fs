namespace CardManagement.Data

module UsersRepository =
    open CardManagement.Infrastructure.DomainModels
    open SqlHydra.Query
    open CardManagement.Data.DatabaseContext
    open CardManagement.Data.``public``
    open System.Threading.Tasks
    open CardManagement.Data.Mappers
    open System
    
    let tryFindUserByEmail (email: string): Task<users option> = task {
        let! users = selectTask HydraReader.Read (Create openContext) {
            for user in users do
            where (user.email = email)
            select user
        }
        match Seq.isEmpty users with
        | true -> return None
        | false -> return Some (Seq.item 0 users)
    }
    
    let saveUser (user: User) =
        let dbUser = convertUserToDB user
        insertTask (Create openContext) {
            into users
            entity dbUser
        }


    let getUserByIdWithJoinCards (id: Guid) =
        selectTask HydraReader.Read (Create openContext) {
            for user in users do
            where (user.id = id)
            join card in cards on (user.id = card.user_id)
            select (user, card)
        }