module CardManagement.Shared.Core

open System
open CardManagement.Shared.Types

type IUsersStore = {
    Register: InputUser -> Async<ResponseResult<RegistrationResponse>>
    Login: string -> string -> Async<ResponseResult<RegistrationResponse>>
}

type ICardsStore = {
    Get: unit -> Async<ResponseResult<Card seq>>
    Create: TypeOfCard -> Async<ResponseResult<Card>>
    CreateTransaction: TransactionInput -> Async<ResponseResult<Transaction>>
}

type IProfileStore = {
    GetMyProfile: unit -> Async<ResponseResult<User>>
}