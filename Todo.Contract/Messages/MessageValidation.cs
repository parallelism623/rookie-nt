using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Contract.Messages;

#region VALIDATE_TODOITEM
public static partial class MessageValidation
{
    public static string TitleCannotEmpty = "Title cannot empty";
    public static string PointShouldBeInRange = "Point should be in range 1 - 10";
    public static string PriorityShouldBeInRange = "Priority should have value low/medium/high";
    public static string IdCannotEmpty = "Id cannot empty";
}

#endregion VALIDATE_TODOITEM
