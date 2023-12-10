namespace CardManagement.API

module SharedTypes =
    
    type Error = {
        message: string
    }
    
    type Result<'a> =
        | Ok of 'a
        | Error of Error
        

