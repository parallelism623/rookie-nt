using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Exceptions;
public class InvalidModelRequestException : ExceptionBase
{
    public InvalidModelRequestException(string message) : base(message)
    {
    }
}
