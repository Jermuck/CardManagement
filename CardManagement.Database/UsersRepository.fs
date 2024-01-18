module CardManagement.Database.UsersRepository

open CardManagement.Shared.Types
open CardManagement.Database.DatabaseContext
open CardManagement.Database.``public``
open CardManagement.Database.Mappers
open SqlHydra.Query
open System

let tryFindUserByEmail (email: string) = async {
    let! users = selectAsync HydraReader.Read sqlHydraContext {
        for user in users do
        where (user.email = email)
        select user
    }
    match Seq.isEmpty users with
    | true -> return None
    | false -> return Seq.item 0 users |> mapDBUserToDomain |> Some
}

let tryFindUserById (id: Guid) = async {
    let! users = selectAsync HydraReader.Read sqlHydraContext {
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
    insertTask sqlHydraContext {
        into users
        entity dbUser
    }


let getUserByIdWithJoinCards (id: Guid) =
    selectAsync HydraReader.Read sqlHydraContext {
        for user in users do
        where (user.id = id)
        join card in cards on (user.id = card.user_id)
        select (user, card)
    }