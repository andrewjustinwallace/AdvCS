/*
 * This program demonstrates a simple validation system using delegates in C#.
 * It implements a flexible validation framework that can chain multiple
 * validation rules together using delegates.
 * 
 * Key concepts demonstrated:
 * 1. Custom delegate definition (ValidationDelegate)
 * 2. Delegate list management
 * 3. Lambda expressions for validation rules
 * 4. LINQ All() method for validation chaining
 * 5. Password validation example
 * 
 * The example shows:
 * - Creating a custom validation delegate
 * - Adding multiple validation rules
 * - Using lambda expressions for validation logic
 * - Chaining validation rules together
 * - Simple password validation implementation
 */

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