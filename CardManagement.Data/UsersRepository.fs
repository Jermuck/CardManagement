namespace CardManagement.Data


module UsersRepository =
    open CardManagement.Infrastructure.DomainModels
    open SqlHydra.Query
    open CardManagement.Data.DatabaseContext
    open CardManagement.Data.``public``
    open System
    
    
    let createUser (user: User) =
        insertTask (Create openContext) {
            into users
            entity {
                id = user.Id
                name = user.Name
                surname = user.Surname
                patronymic = user.Patronymic
                salary = user.Salary
                age = user.Age
                email = user.Email
            }
        }


    let getUserByIdWithJoinCards (id: Guid) =
        selectTask HydraReader.Read (Create openContext) {
            for user in users do
            where (user.id = id)
            join card in cards on (user.id = card.user_id)
            select (user, card)
        }