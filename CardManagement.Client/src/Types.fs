module CardManagement.Client.Types

open System
open CardManagement.Shared.Types
open Fable.React

type InputType =
    | Text
    | Password
    
type TypeAuthorization =
    | Login
    | Registration
    
type ICardForm = {
    CardElement: ReactElement
    TypeCard: TypeOfCard
    TagText: string
    Content: string
    onClick: TypeOfCard -> unit
}

type IMessage = {
    Message: string
    Color: string
}

type SortingArg =
    | BasicCard
    | PriorityCard
    | All

type IHeadersTabs = {
    Id: Guid
    Text: string
    ClassName: string
    Type: SortingArg
}

type ITransactionModalComponent = {
    Message: string
    Amount: int
    Code: string
    CardIdSender: Guid
}