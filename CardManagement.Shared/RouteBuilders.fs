module CardManagement.Shared.RouteBuilders

let BuildApiRoute (typeName: string) (methodName: string) = 
    $"/api/%s{typeName}/%s{methodName}"