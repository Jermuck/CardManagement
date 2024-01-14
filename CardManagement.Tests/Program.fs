open System
open System.Numerics
open CardManagement.Data.UsersRepository

[<EntryPoint>]
let main _ =
    let code = int64(Math.Abs(Random().Next(100_000_000, 999_999_999))) * int64(100000)
    printfn "%A" (code)
    0 
