module CardManagement.Database.UsersRepository

open SqlHydra.Query
open CardManagement.Shared
open CardManagement.Database
open Types
open DatabaseContext
open ``public``
open Mappers

let tryFindUserByEmail email = async {
    let! users = selectAsync HydraReader.Read (Create openContext) {
        for user in users do
        where (user.email = email)
        select user
    }
    match Seq.isEmpty users with
    | true -> return None
    | false -> return Seq.item 0 users |> mapDBUserToDomain |> Some
}

let tryFindUserById id = async {
    let! users = selectAsync HydraReader.Read (Create openContext) {
        for user in users do
        where (user.id = id)
        select user
    }
    match Seq.isEmpty users with
    | true -> return None
    | false -> return Seq.item 0 users |> mapDBUserToDomain |> Some 
}

let saveUser user =
    let dbUser = mapUserToDB user
    insertTask (Create openContext) {
        into users
        entity dbUser
    }

let updateUser (inputUser: InputUser) id =
    updateAsync (Create openContext) {
        for user in users do
        where (user.id = id)
        set user.name inputUser.Name
        set user.surname inputUser.Surname
        set user.patronymic inputUser.Patronymic
        set user.email inputUser.Email
        set user.age inputUser.Age
        set user.salary inputUser.Salary
    }

let getUserByIdWithJoinCards id =
    selectAsync HydraReader.Read (Create openContext) {
        for user in users do
        where (user.id = id)
        join card in cards on (user.id = card.user_id)
        select (user, card)
    }