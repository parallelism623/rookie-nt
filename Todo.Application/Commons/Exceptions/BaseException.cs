namespace Todo.Application.Commons.Exceptions;
public abstract class BaseException : Exception
{
    protected BaseException(string message) : base(message)
    { }
}
