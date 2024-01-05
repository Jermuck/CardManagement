module CardManagement.Server.ServerConfiguration

open System
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Http
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open CardManagement.API.UserStore
open CardManagement.Shared.RouteBuilders
open Giraffe
   
let createHandler controller = 
    Remoting.createApi()
    |> Remoting.withRouteBuilder BuildApiRoute
    |> Remoting.fromValue controller
    |> Remoting.buildHttpHandler

//let authenticate =
  //  requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

let webApi: HttpFunc -> HttpContext -> HttpFuncResult =
    [
        createHandler usersStoreImplementation
        createHandler privateStoreImplementation
    ] |> choose

let UseCors (builder: CorsPolicyBuilder) =
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        |> ignore

let configureApp (app : IApplicationBuilder) =
    app.UseCors(Action<_> UseCors) |> ignore
    // app.UseAuthentication() |> ignore
    app.UseGiraffe webApi
    

let configureServices (services : IServiceCollection) =
    services.AddCors() |> ignore
    // services.AddAuthentication(fun opt ->
    //     opt.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
    //     opt.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme
    //     )
    //     .AddJwtBearer(fun opt ->
    //         opt.TokenValidationParameters <- TokenValidationParameters(
    //             IssuerSigningKey = convertKeyToBytes key,
    //             ValidateIssuer = true,
    //             ValidIssuer = issuer,
    //             ValidateAudience = true,
    //             ValidAudience = audience
    //             )
    //         ) |> ignore
    services.AddGiraffe() |> ignore
    
