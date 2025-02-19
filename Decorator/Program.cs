/*
 * This program demonstrates the Decorator design pattern using a coffee shop example.
 * The Decorator pattern allows behavior to be added to individual objects dynamically
 * without affecting the behavior of other objects from the same class.
 * 
 * Key concepts demonstrated:
 * 1. Component Interface (ICoffee) defining base operations
 * 2. Concrete Component (SimpleCoffee) implementing base functionality
 * 3. Decorator Base Class (CoffeeDecorator) maintaining reference to decorated object
 * 4. Concrete Decorators (Milk, Sugar, WhippedCream) adding behavior
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each decorator has one responsibility (adding one specific feature)
 *    - Base coffee class handles only core coffee functionality
 *    - Each class has a single reason to change
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New behaviors can be added through new decorators
 *    - Existing classes remain unchanged when adding features
 *    - System is open for extension but closed for modification
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All decorators can be used wherever ICoffee is expected
 *    - Decorated objects preserve the base contract
 *    - Behavior remains consistent with interface expectations
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - ICoffee interface defines minimal required methods
 *    - Decorators implement only necessary functionality
 *    - Clients depend only on methods they use
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - High-level modules depend on ICoffee abstraction
 *    - Decorators work with ICoffee interface, not concrete classes
 *    - Components are loosely coupled through abstractions
 * 
 * Pattern benefits:
 * - Supports Open/Closed Principle
 * - Provides flexible alternative to subclassing
 * - Allows responsibilities to be added/removed at runtime
 * - Promotes single responsibility principle
 * 
 * The example shows how to:
 * - Create a basic component (simple coffee)
 * - Add features dynamically (milk, sugar, whipped cream)
 * - Stack multiple decorators
 * - Calculate cumulative costs
 */

using System;
using System.Collections.Generic;

namespace DecoratorPatternDemo
{
    // Component interface
    public interface ICoffee
    {
        string GetDescription();
        double GetCost();
    }

    // Concrete Component
    public class SimpleCoffee : ICoffee
    {
        public string GetDescription()
        {
            return "Simple Coffee";
        }

        public double GetCost()
        {
            return 1.0;
        }
    }

    // Decorator base class
    public abstract class CoffeeDecorator : ICoffee
    {
        protected ICoffee _coffee;

        public CoffeeDecorator(ICoffee coffee)
        {
            _coffee = coffee;
        }

        public virtual string GetDescription()
        {
            return _coffee.GetDescription();
        }

        public virtual double GetCost()
        {
            return _coffee.GetCost();
        }
    }

    // Concrete Decorators
    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(ICoffee coffee) : base(coffee) { }

        public override string GetDescription()
        {
            return $"{_coffee.GetDescription()}, Milk";
        }

        public override double GetCost()
        {
            return _coffee.GetCost() + 0.5;
        }
    }

    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(ICoffee coffee) : base(coffee) { }

        public override string GetDescription()
        {
            return $"{_coffee.GetDescription()}, Sugar";
        }

        public override double GetCost()
        {
            return _coffee.GetCost() + 0.2;
        }
    }

    public class WhippedCreamDecorator : CoffeeDecorator
    {
        public WhippedCreamDecorator(ICoffee coffee) : base(coffee) { }

        public override string GetDescription()
        {
            return $"{_coffee.GetDescription()}, Whipped Cream";
        }

        public override double GetCost()
        {
            return _coffee.GetCost() + 0.7;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Decorator Pattern Demo: Coffee Shop");
            Console.WriteLine("===================================");

            // Create a simple coffee
            ICoffee simpleCoffee = new SimpleCoffee();
            PrintCoffeeDetails(simpleCoffee);

            // Add milk to the coffee
            ICoffee milkCoffee = new MilkDecorator(simpleCoffee);
            PrintCoffeeDetails(milkCoffee);

            // Add sugar to the milk coffee
            ICoffee sweetMilkCoffee = new SugarDecorator(milkCoffee);
            PrintCoffeeDetails(sweetMilkCoffee);

            // Create a complex coffee with multiple decorators
            ICoffee ultimateCoffee = new WhippedCreamDecorator(
                                        new SugarDecorator(
                                            new MilkDecorator(
                                                new SimpleCoffee())));
            PrintCoffeeDetails(ultimateCoffee);

            Console.ReadLine();
        }

        static void PrintCoffeeDetails(ICoffee coffee)
        {
            Console.WriteLine($"\nYour coffee: {coffee.GetDescription()}");
            Console.WriteLine($"Cost: ${coffee.GetCost():F2}");
        }
    }
}