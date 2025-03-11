namespace Identity.Application.Dtos
{
    public record AuthenticationDto(string AccessToken, string RefreshToken);
}
