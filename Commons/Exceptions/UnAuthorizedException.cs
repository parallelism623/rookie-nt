namespace mvc_todolist.Commons.Exceptions
{
    public class UnAuthorizedException : BaseException
    {
        public UnAuthorizedException(string message) : base(message)
        {
        }
    }
}
