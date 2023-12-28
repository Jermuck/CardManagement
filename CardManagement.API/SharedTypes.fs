namespace CardManagement.API

module SharedTypes =
    open CardManagement.Infrastructure.InputTypes
    open CardManagement.Infrastructure.DomainModels
    
    type Error = {
        Message: string
    }
    
    type Result<'a> =
        | Ok of 'a
        | Error of Error
    
    type RegistrationResponse = User * Token
    
    type IUsersStore = {
        Register: InputUser -> Async<Result<RegistrationResponse>>
    }
    