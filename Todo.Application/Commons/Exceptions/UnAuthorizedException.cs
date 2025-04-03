using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Commons.Exceptions;
public class UnAuthorizedException : BaseException
{
    public UnAuthorizedException(string message) : base(message)
    {
    }
}
