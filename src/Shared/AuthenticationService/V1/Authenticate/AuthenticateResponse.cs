namespace AuthenticationService.V1.Authenticate;

public sealed record AuthenticateResponse(bool IsAuthenticated, string? ErrorMessage = null) { }
