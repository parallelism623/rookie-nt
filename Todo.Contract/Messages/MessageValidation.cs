namespace Todo.Contract.Messages;

#region VALIDATE_TODOITEM
public static partial class MessageValidation
{
    public const string TitleCannotEmpty = "Title cannot empty";
    public const string PointShouldBeInRange = "Point should be in range 1 - 10";
    public const string PriorityShouldBeInRange = "Priority should have value low/medium/high";
    public const string IdCannotEmpty = "Id cannot empty";
}

#endregion VALIDATE_TODOITEM
