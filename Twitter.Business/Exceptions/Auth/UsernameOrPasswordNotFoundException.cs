namespace Twitter.Business.Exceptions.Auth;
public class UsernameOrPasswordNotFoundException : Exception
{
    public UsernameOrPasswordNotFoundException() : base("Username Or Password Is Wrong") { }

    public UsernameOrPasswordNotFoundException(string? message) : base(message)
    {
    }
}
