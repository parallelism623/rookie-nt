namespace Todo.Application.Commons.Exceptions;
public class UnAuthorizedException : BaseException
{
    public UnAuthorizedException(string message) : base(message)
    {
    }
}
