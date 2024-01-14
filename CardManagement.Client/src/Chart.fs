module CardManagement.Client.Chart

open Feliz
open Feliz.Recharts

type Point = { name: string; uv: int; pv: int }

let data = [
    { name = "Page A"; uv = 4000; pv = 2400 }
    { name = "Page B"; uv = 3000; pv = 1398 }
    { name = "Page C"; uv = 2000; pv = 9800 }
    { name = "Page D"; uv = 2780; pv = 3908 }
    { name = "Page E"; uv = 1890; pv = 4800 }
    { name = "Page F"; uv = 2390; pv = 3800 }
    { name = "Page G"; uv = 3490; pv = 4300 }
]

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
let Chart width height =
    Recharts.areaChart [
        areaChart.width width
        areaChart.height height
        areaChart.data data
        areaChart.margin(top=10, right=30)
        areaChart.children [
            Svg.defs [
                createGradient "colorUv" "#8884d8"
                createGradient "colorPv" "#82ca9d"
            ]
            Recharts.xAxis [ xAxis.dataKey (fun point -> point.name) ]
            Recharts.yAxis [ ]
            Recharts.tooltip [ ]
            Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]

            Recharts.area [
                area.monotone
                area.dataKey (fun point -> point.uv)
                area.stroke "#8884d8"
                area.fillOpacity 1
                area.fill "url(#colorUv)"
            ]

            Recharts.area [
                area.monotone
                area.dataKey (fun point -> point.pv)
                area.stroke "#82ca9d"
                area.fillOpacity 1
                area.fill "url(#colorPv)"
            ]
        ]
    ]
