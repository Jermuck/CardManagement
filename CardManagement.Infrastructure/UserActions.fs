namespace CardManagement.Infrastructure

module UserActions =
    open CardManagement.Infrastructure.InputTypes
    open CardManagement.Infrastructure.DomainModels
    open System
    
    let buildUser (inputUser: InputUser): User =
        {
            Id = Guid.NewGuid()
            Name = inputUser.Name
            Surname = inputUser.Surname
            Patronymic = inputUser.Patronymic
			Password = inputUser.Password
            Age = inputUser.Age
            Salary = inputUser.Salary
            Email = inputUser.Email
            Cards = [] 
        }
    