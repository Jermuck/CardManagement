module CardManagement.Server.ChartStore

open CardManagement.Shared.Core
open CardManagement.Shared.Types
open CardManagement.Shared.Utils
open CardManagement.Database.TransactionsRepository

let private getCoordinates cardId = async {
    try
        let! transactionsByCard = getTransactionsByCardId cardId
        let! transactionsToUserId = getTransactionsToCardId cardId
        let allTransactions = Seq.concat [transactionsByCard; transactionsToUserId] |> Seq.sortBy (_.CreateDate)
        let mapToPoint (transaction: Transaction) =
            let uv =
                allTransactions
                |> Seq.filter (fun v -> v.CreateDate.Day = transaction.CreateDate.Day && v.CardId = cardId)
                |> Seq.sumBy(_.Sum)
            let pv =
                allTransactions
                |> Seq.filter (fun v -> v.CreateDate.Day = transaction.CreateDate.Day && v.ToCardId = cardId)
                |> Seq.sumBy(_.Sum)
            {
                Name = transaction.CreateDate.Day.ToString() + " " + (getStringMonth transaction.CreateDate)[..2] + " " + transaction.CreateDate.Year.ToString()[2..]
                Pv = pv
                Uv = uv
            }
        let points = allTransactions |> Seq.map mapToPoint |> Seq.distinct
        return Ok points
    with
        | ex -> printfn "%A" ex; return Error { Message = "Server error" }
}

let getChartStoreImplementation _: IChartStore =
    {
        GetCoordinates = getCoordinates
    }

