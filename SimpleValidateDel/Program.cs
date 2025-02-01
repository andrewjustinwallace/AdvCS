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
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - ValidationDelegate focuses solely on validation logic
 *    - RegistrationValidator manages only validation rule collection
 *    - Each validation rule handles one specific check
 *    - Lambda expressions encapsulate single validation logic
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New validation rules can be added without modifying existing code
 *    - Validator class accepts any validation rule meeting delegate signature
 *    - Validation chain can be extended without changing implementation
 *    - Lambda expressions allow flexible rule definition
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All validation methods follow delegate signature
 *    - Validation rules are interchangeable in the chain
 *    - Lambda expressions maintain consistent behavior
 *    - Rules can be substituted without affecting validation chain
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - ValidationDelegate defines minimal required method
 *    - Validation rules implement only necessary logic
 *    - Clients depend only on validations they need
 *    - Each rule focuses on specific validation aspect
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Validator depends on ValidationDelegate abstraction
 *    - Rules are passed as delegate instances
 *    - Implementation details are decoupled through delegation
 *    - Concrete validation logic is injected at runtime
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