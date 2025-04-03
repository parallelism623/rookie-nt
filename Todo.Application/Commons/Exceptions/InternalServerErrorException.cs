using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Commons.Exceptions;
public class InternalServerErrorException : BaseException
{
    public InternalServerErrorException(string message) : base(message)
    {
    }
}
