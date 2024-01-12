module CardManagement.Shared.Core

open CardManagement.Shared.Types

type IUsersStore = {
    Register: InputUser -> Async<ResponseResult<RegistrationResponse>>
    Login: string -> string -> Async<ResponseResult<RegistrationResponse>>
}

type ICardsStore = {
    Get: unit -> Async<ResponseResult<Card seq>>
    Create: TypeOfCard -> Async<ResponseResult<Card>>
}

type IProfileStore = {
    GetMyProfile: unit -> Async<ResponseResult<User>>
}