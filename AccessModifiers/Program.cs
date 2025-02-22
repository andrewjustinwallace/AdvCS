/*
 * This program demonstrates the different access modifiers in C# and their scope of accessibility.
 * It provides practical examples of how each modifier affects access to class members from:
 * - Within the same class
 * - Derived classes
 * - External classes within the same assembly
 * 
 * Access Modifiers Demonstrated:
 * 1. public: Accessible from anywhere
 * 2. private: Only within the same class/struct
 * 3. protected: Within same class and derived classes
 * 4. internal: Within same assembly
 * 5. protected internal: Within assembly and derived classes in other assemblies
 * 6. private protected: Within class and derived classes in same assembly
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Access modifiers help enforce SRP by controlling where properties and methods can be accessed
 *    - Private members ensure that internal implementation details stay encapsulated
 *    - Protected members allow selective sharing of functionality with derived classes
 * 
 * 2. Open/Closed Principle (OCP):
 *    - Protected members enable extension through inheritance while keeping base implementation closed
 *    - Internal access allows assembly-level extension without exposing to external code
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Protected members ensure derived classes have access to necessary base class functionality
 *    - Access levels help maintain proper inheritance hierarchies
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Access modifiers help hide unnecessary implementation details
 *    - Different access levels allow fine-grained control over what clients can see
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Internal and protected internal enable assembly-level abstractions
 *    - Public interfaces with private implementations support dependency inversion
 * 
 * The program uses inheritance to show how access changes in different contexts
 * and includes examples of both legal and illegal (commented out) access attempts.
 */

using System;

namespace AccessModifiersDemo
{
    public class BaseClass
    {
        public int PublicField = 1;
        private int PrivateField = 2;
        protected int ProtectedField = 3;
        internal int InternalField = 4;
        protected internal int ProtectedInternalField = 5;
        private protected int PrivateProtectedField = 6;

        public void DisplayFields()
        {
            Console.WriteLine($"Public: {PublicField}");
            Console.WriteLine($"Private: {PrivateField}");
            Console.WriteLine($"Protected: {ProtectedField}");
            Console.WriteLine($"Internal: {InternalField}");
            Console.WriteLine($"Protected Internal: {ProtectedInternalField}");
            Console.WriteLine($"Private Protected: {PrivateProtectedField}");
        }
    }

    public class DerivedClass : BaseClass
    {
        public void AccessBaseClassFields()
        {
            Console.WriteLine($"Public: {PublicField}");
            // Console.WriteLine($"Private: {PrivateField}"); // This would cause a compilation error
            Console.WriteLine($"Protected: {ProtectedField}");
            Console.WriteLine($"Internal: {InternalField}");
            Console.WriteLine($"Protected Internal: {ProtectedInternalField}");
            Console.WriteLine($"Private Protected: {PrivateProtectedField}");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BaseClass baseObj = new BaseClass();
            baseObj.DisplayFields();

            Console.WriteLine("\nAccessing from outside the class:");
            Console.WriteLine($"Public: {baseObj.PublicField}");
            // Console.WriteLine($"Private: {baseObj.PrivateField}"); // This would cause a compilation error
            // Console.WriteLine($"Protected: {baseObj.ProtectedField}"); // This would cause a compilation error
            Console.WriteLine($"Internal: {baseObj.InternalField}");
            Console.WriteLine($"Protected Internal: {baseObj.ProtectedInternalField}");
            // Console.WriteLine($"Private Protected: {baseObj.PrivateProtectedField}"); // This would cause a compilation error

            Console.WriteLine("\nAccessing from derived class:");
            DerivedClass derivedObj = new DerivedClass();
            derivedObj.AccessBaseClassFields();
        }
    }
}