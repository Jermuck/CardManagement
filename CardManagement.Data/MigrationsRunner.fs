namespace CardManagement.Data

module MigrationsRunner =
   open DbUp
   open CardManagement.Data.Configuration
   let scriptsFolderPath = "../CardManagement.Data/Migrations"
   let migrations =
        DeployChanges.To
            .PostgresqlDatabase(connection)
            .WithScriptsFromFileSystem(scriptsFolderPath)
            .LogToConsole()
            .Build();

