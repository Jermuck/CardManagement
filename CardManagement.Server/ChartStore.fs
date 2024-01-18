module CardManagement.Server.ChartStore

open CardManagement.Shared.Core
open CardManagement.Shared.Types
open CardManagement.Server.RemotingUtils
open CardManagement.Database.TransactionsRepository

let private getCoordinates userId cardId = async {
    try
        let! transactionsByCard = getTransactionsByCardId cardId
        let! transactionsToUserId = getTransactionsToUserId userId cardId
        let allTransactions = Seq.concat [transactionsByCard; transactionsToUserId] |> Seq.sortBy (_.CreateDate)
        let mapToPoint (transaction: Transaction) =
            let moneyInOnDay =
                allTransactions
                |> Seq.filter (fun v -> v.CreateDate.Day = transaction.CreateDate.Day && v.CardId = cardId)
            let uv =
                match Seq.isEmpty moneyInOnDay with
                | false -> (Seq.sumBy (_.Sum) moneyInOnDay) / Seq.length moneyInOnDay |> float
                | true -> 0
            let moneyOutOnDay =
                allTransactions
                |> Seq.filter (fun v -> v.CreateDate.Day = transaction.CreateDate.Day && v.ToUserId = userId && v.CardId <> cardId)
            let pv =
                match Seq.isEmpty moneyOutOnDay with
                | false -> (Seq.sumBy (_.Sum) moneyOutOnDay) / Seq.length moneyOutOnDay |> float
                | true -> 0
            {
                Name = transaction.CreateDate.Day.ToString() + ":" + transaction.CreateDate.Month.ToString() + ":" + transaction.CreateDate.Year.ToString()
                Pv = pv
                Uv = uv
            }
        let points = allTransactions |> Seq.map mapToPoint |> Seq.distinct
        return Ok points
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getChartStoreImplementation ctx: IChartStore =
    let userId = getUserIdFromHttpContext ctx
    {
        GetCoordinates = getCoordinates userId 
    }

