module CardManagement.Client.ChartComponent

open Feliz
open Feliz.Recharts
open Feliz.UseDeferred
open CardManagement.Shared
open CardManagement.Client
open WebApi
open Types

[<ReactComponent>]
let createGradient (id: string) color =
    Svg.linearGradient [
        svg.id id
        svg.x1 0; svg.x2 0
        svg.y1 0; svg.y2 1
        svg.children [
            Svg.stop [
                svg.offset(length.percent 5)
                svg.stopColor color
                svg.stopOpacity 0.8
            ]
            Svg.stop [
                svg.offset(length.percent 95)
                svg.stopColor color
                svg.stopOpacity 0.0
            ]
        ]
    ]

[<ReactComponent>]
let ChartComponent (width: int) (height: int) cardId =
    let getData() = async {
        let! result = chartStore.GetCoordinates cardId
        match result with
        | Error _ -> return []
        | Ok v -> return Seq.toList v
    }
    
    let request = React.useDeferred(getData(), [|box cardId|])
    
    match request with
    | Deferred.Resolved data ->
         match data.IsEmpty with
         | true ->
            Html.div [
                prop.style [
                    style.width width
                    style.height height
                    style.display.flex
                    style.justifyContent.center
                    style.alignItems.center
                ]
                prop.children [
                    Html.h1 [
                        prop.style [
                            style.marginBottom 28
                        ]
                        prop.text "Empty Insights"
                    ]
                ]
            ]
         | false -> 
             Recharts.areaChart [
                areaChart.width width
                areaChart.height height
                areaChart.data data
                areaChart.margin(top=10, right=30)
                areaChart.children [
                    Svg.defs [
                        createGradient "colorUv" "red"
                        createGradient "colorPv" "#82ca9d"
                    ]
                    Recharts.xAxis [ xAxis.dataKey (fun point -> point.Name) ]
                    Recharts.yAxis [ ]
                    Recharts.tooltip [ ]
                    Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]
                    Recharts.area [
                        area.monotone
                        area.dataKey (fun point -> point.Uv)
                        area.stroke "red"
                        area.fillOpacity 1
                        area.fill "url(#colorUv)"
                    ]
                    Recharts.area [
                        area.monotone
                        area.dataKey (fun point -> point.Pv)
                        area.stroke "#82ca9d"
                        area.fillOpacity 1
                        area.fill "url(#colorPv)"
                    ]
                ]
            ]
    | _ -> Html.none