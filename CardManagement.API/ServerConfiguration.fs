namespace CardManagement.API

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
      //  requiresAuthentication (RequestErrors.UNAUTHORIZED JwtBearerDefaults.AuthenticationScheme "" "User not logged in")
    
    let webApp =
        [
            createHandler usersStoreImplementation 
        ] |> choose
    
    let configureApp (app : IApplicationBuilder) =
        app.UseCors(fun builder ->
               builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() |> ignore
            ) |> ignore
        app.UseAuthentication() |> ignore
        app.UseGiraffe webApp
        

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
        
    