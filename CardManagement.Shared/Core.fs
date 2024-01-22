module CardManagement.Shared.Core

open System
open CardManagement.Shared.Types

type IUsersStore = {
    Register: InputUser -> Async<ResponseResult<RegistrationResponse>>
    Login: string -> string -> Async<ResponseResult<RegistrationResponse>>
}

type ICardsStore = {
    GetCards: unit -> Async<ResponseResult<Card seq>>
    CreateCard: TypeOfCard -> Async<ResponseResult<Card>>
    CreateTransaction: TransactionInput -> Async<ResponseResult<Transaction>>
    GetTransactions: Guid -> Async<ResponseResult<Transaction seq>>
    BlockCard: Guid -> Async<ResponseResult<Card>>
}

type IProfileStore = {
    GetMyProfile: unit -> Async<ResponseResult<User>>
    UpdateProfile: InputUser -> Async<ResponseResult<string>>
}

type IChartStore = {
    GetCoordinates: Guid -> Async<ResponseResult<Point seq>>
}