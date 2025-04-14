namespace Rookies.Contract.Messages;

public static class LoggingTemplateMessages
{
    public const string PersonCreatedWithIdSuccess = "Person with id {Id} created.";

    public const string PersonDeletedWithIdSuccess = "Person with id {Id} deleted.";

    public const string PersonUpdatedWithDataSuccess = "Person with id {Id} updated - Data {@Data}.";

    public const string PersonQuerySuccess = "Retrieved {Count} persons.";
}