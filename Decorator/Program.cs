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