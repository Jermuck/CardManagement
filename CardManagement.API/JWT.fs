namespace CardManagement.API

module JWT =
    open System
    open System.IdentityModel.Tokens.Jwt
    open Microsoft.IdentityModel.Tokens
    open CardManagement.API.ConfigService
    open System.Security.Claims
    open System.Text
    open CardManagement.Infrastructure.DomainModels
    
    let key = getValue "AppSettings:Key"
    let issuer = getValue "AppSettings:Issuer"
    let audience = getValue "AppSettings:Audience"
    let expirence = 60 * 24 * 31 * 10000
    
    let convertKeyToBytes (key: string) =
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    
    let private generateToken claims = 
        let credentials = SigningCredentials(convertKeyToBytes key, SecurityAlgorithms.HmacSha256)
        let issuedOn = DateTimeOffset.UtcNow
        let expiresOn = issuedOn.Add(TimeSpan.FromMilliseconds(expirence))
        let jwtToken = JwtSecurityToken(issuer, audience, claims, (issuedOn.UtcDateTime |> Nullable), (expiresOn.UtcDateTime |> Nullable), credentials)
        let handler = JwtSecurityTokenHandler()
        let token = handler.WriteToken(jwtToken)
        { Token = token }
        
    let userToToken (user: User) =
        [|Claim("Id", user.Id.ToString())|]
        |> generateToken
        
    