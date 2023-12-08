namespace CardManagement.API

module SharedTypes =
    open System
    
    type UserDTO = {
        Name: string
        Surname: string
        Patronymic: string
        Age: int
        Salary: int
        Email: string
    }
    
    type Error = {
        message: string
    }
        

