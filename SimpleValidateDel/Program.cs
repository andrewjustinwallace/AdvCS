

public delegate bool ValidationDelegate(string input);

public class RegistrationValidator
{
    private readonly List<ValidationDelegate> _validations = new List<ValidationDelegate>();

    public void AddValidation(ValidationDelegate validation)
    {
        _validations.Add(validation);
    }

    public bool Validate(string input)
    {
        return _validations.All(validation => validation(input));
    }
}

class Program {

    public static void Main(string[] args)
    {
        var validator = new RegistrationValidator();
        validator.AddValidation(input => !string.IsNullOrEmpty(input));
        validator.AddValidation(input => input.Length >= 8);
        validator.AddValidation(input => input.Any(char.IsUpper));
        bool isValid = validator.Validate("Password123");

        Console.WriteLine(isValid);
        Console.ReadKey();
    }
}