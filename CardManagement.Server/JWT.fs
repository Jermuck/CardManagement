module CardManagement.Server.JWT

open System
open System.Text
open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open Microsoft.IdentityModel.Tokens
open CardManagement.Shared.Types
open CardManagement.Server.ConfigService

let key = getValue "AppSettings:Key"
let issuer = getValue "AppSettings:Issuer"
let audience = getValue "AppSettings:Audience"
let exp = 60 * 24 * 31 * 10000

let convertKeyToBytes (key: string) =
    SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))

let private generateToken claims = 
    let credentials = SigningCredentials(convertKeyToBytes key, SecurityAlgorithms.HmacSha256)
    let issuedOn = DateTimeOffset.UtcNow
    let expiresOn = issuedOn.Add(TimeSpan.FromMilliseconds(exp))
    let jwtToken = JwtSecurityToken(issuer, audience, claims, (issuedOn.UtcDateTime |> Nullable), (expiresOn.UtcDateTime |> Nullable), credentials)
    let handler = JwtSecurityTokenHandler()
    let token = handler.WriteToken(jwtToken)
    { Token = token }
    
let userToToken (user: User) =
    [|Claim("Id", user.Id.ToString())|]
    |> generateToken
        
    