namespace Todo.Application.Commons.Exceptions;
public class InternalServerErrorException : BaseException
{
    public InternalServerErrorException(string message) : base(message)
    {
    }
}
