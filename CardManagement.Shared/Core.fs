module CardManagement.Shared.Core

open Types

type IUsersStore = {
    Register: InputUser -> Async<ResponseResult<RegistrationResponse>>
}

type IPrivateStore = {
    Get: unit -> Async<string>
}
