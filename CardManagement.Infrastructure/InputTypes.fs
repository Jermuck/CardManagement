namespace CardManagement.Infrastructure

module InputTypes =
    
    type InputUser = {
        Name: string
        Surname: string
        Patronymic: string
        Age: int
        Salary: int
        Email: string
    }

