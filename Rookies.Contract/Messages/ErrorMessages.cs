namespace Rookies.Contract.Messages;

public static class ErrorMessages
{
    public const string ErrorMessageInvalidLengthFirstName = "First name must be between 1 and 50 characters.";

    public const string ErrorMessageInvalidLengthLastName = "Last name must be between 1 and 50 characters.";

    public const string GenderInvalid = "Gender is invalid";

    public const string FirstNameInvalid = "FirstName is invalid format";

    public const string LastNameInvalid = "LastName is invalid format";

    public const string BirthPlaceInvalidLength = "Birth place must be between 1 and 100 characters.";

    public const string PersonIdRequire = "Person id is required";

    public const string PersonNotFoundById = "Person with id {0} not found.";

    public const string PersonIdShouldNotEmpty = "Person id should not empty";

    public const string EmailAddressInvalid = "Email is invalid";

    public const string PhoneNumberInvalid = "Phone number is invalid";

    public const string EmailShouldNotBeEmpty = "Email should not be empty";

    public const string PhoneNumberShouldNotBeEmpty = "Phone number should not be empty";

    public const string EmailAlreadyExists = "Email already exists";
}