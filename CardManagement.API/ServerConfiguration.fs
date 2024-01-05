namespace CardManagement.API

open System
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Http

module ServerConfiguration =
    open Fable.Remoting.Server
    open Fable.Remoting.Giraffe
    open Microsoft.AspNetCore.Builder
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.AspNetCore.Authentication.JwtBearer
    open CardManagement.API.JWT
    open Giraffe
    open Microsoft.IdentityModel.Tokens
    open CardManagement.API.UserStore
        
    let private builder typeName methodName =
       sprintf "/api/%s/%s" typeName methodName
       
    let createHandler controller = 
        Remoting.createApi()
        |> Remoting.withRouteBuilder builder
        |> Remoting.fromValue controller
        |> Remoting.buildHttpHandler
    
    //let authenticate =
      //  requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)
    
    let webApi: HttpFunc -> HttpContext -> HttpFuncResult =
        [
            createHandler privateStoreImplementation
            createHandler usersStoreImplementation
        ] |> choose
    
    let UseCors (builder: CorsPolicyBuilder) =
        builder
            .AllowAnyOrigin()
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
        
    