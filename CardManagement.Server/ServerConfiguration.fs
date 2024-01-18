module CardManagement.Server.ServerConfiguration

open System
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open CardManagement.Server.UserStore
open CardManagement.Server.JWT
open CardManagement.Server.RemotingUtils
open CardManagement.Server.ProfileStore
open CardManagement.Server.CardsStore
open CardManagement.Server.ChartStore
open Giraffe
open Microsoft.IdentityModel.Tokens

let authenticate: HttpFunc -> HttpContext -> HttpFuncResult =
    requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

let webApi: HttpFunc -> HttpContext -> HttpFuncResult =
    [
        createPublicHandler usersStoreImplementation
        authenticate >=> choose [
            createPrivateHandler getProfileStoreImplementation
            createPrivateHandler getCardsStoreImplementation
            createPrivateHandler getChartStoreImplementation
        ]
    ] |> choose

let UseCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        |> ignore

let configureApp (app : IApplicationBuilder) =
    app.UseCors(Action<_> UseCors) |> ignore
    app.UseAuthentication() |> ignore
    app.UseGiraffe webApi
    

let configureServices (services : IServiceCollection) =
    services.AddCors() |> ignore
    services.AddAuthentication(fun opt ->
         opt.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
         opt.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme
         )
         .AddJwtBearer(fun opt ->
             opt.TokenValidationParameters <- TokenValidationParameters(
                 IssuerSigningKey = convertKeyToBytes key,
                 ValidateIssuer = true,
                 ValidIssuer = issuer,
                 ValidateAudience = true,
                 ValidAudience = audience
                 )
             ) |> ignore
    services.AddGiraffe() |> ignore
    
