module CardManagement.Data.UsersRepository

open CardManagement.Shared.Types
open CardManagement.Data.DatabaseContext
open CardManagement.Data.``public``
open CardManagement.Data.Mappers
open System.Threading.Tasks
open SqlHydra.Query
open System

let tryFindUserByEmail (email: string): Task<User option> = task {
    let! users = selectTask HydraReader.Read (Create openContext) {
        for user in users do
        where (user.email = email)
        select user
    }
    match Seq.isEmpty users with
    | true -> return None
    | false -> return Seq.item 0 users |> mapDBUserToDomain |> Some
}

let tryFindUserById (id: Guid): Task<User option> = task {
    let! users = selectTask HydraReader.Read (Create openContext) {
        for user in users do
        where (user.id = id)
        select user
    }
    match Seq.isEmpty users with
    | true -> return None
    | false -> return Seq.item 0 users |> mapDBUserToDomain |> Some 
}

let saveUser (user: User) =
    let dbUser = mapUserToDB user
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