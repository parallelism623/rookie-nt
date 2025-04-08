namespace Todo.Application.Commons.Exceptions;
public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
