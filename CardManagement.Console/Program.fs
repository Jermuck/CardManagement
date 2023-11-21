open CardManagement.Data

[<EntryPoint>]
let main _ =
    let upgrade = MigrationsRunner.migrations.PerformUpgrade()
    printfn "%A" upgrade.Successful
    0