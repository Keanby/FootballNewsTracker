namespace GraphQL.Classes.dto
{
    public record UserLoginInput(string Username,string Password);
    public record UserLoginPayload(string Username, bool Login,string JwtToken);
}
