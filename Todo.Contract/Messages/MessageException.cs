using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Contract.Messages
{
    public static class MessageException
    {
        public static string ItemNotBeExistsCannotDelete = "Cannot delete non-exists item";
        public static string ItemNotExists = "Item don't exists";
    }
}
