//public: Accessible from anywhere, both within the same assembly and from other assemblies.
//private: Accessible only within the same class or struct.
//protected: Accessible within the same class and by derived classes.
//internal: Accessible within the same assembly, but not from other assemblies.
//protected internal: Accessible within the same assembly and by derived classes in other assemblies.
//private protected: Accessible within the same class and by derived classes in the same assembly.


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