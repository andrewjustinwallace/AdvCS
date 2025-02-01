﻿/*
 * This program demonstrates the Template Method design pattern using a beverage preparation example.
 * The Template Method pattern defines the skeleton of an algorithm in a method, deferring some
 * steps to subclasses. It lets subclasses redefine certain steps of an algorithm without
 * changing the algorithm's structure.
 * 
 * Key concepts demonstrated:
 * 1. Abstract base class (Beverage) with template method
 * 2. Concrete implementations (Tea, Coffee)
 * 3. Hook methods (CustomerWantsCondiments)
 * 4. Invariant steps (BoilWater, PourInCup)
 * 5. Abstract operations (Brew, AddCondiments)
 * 
 * Pattern benefits:
 * - Reuses common code in base class
 * - Provides framework for varying parts
 * - Enforces consistent algorithm structure
 * - Enables selective overriding
 * - Supports Hollywood Principle ("don't call us, we'll call you")
 */

using System;

namespace TemplateMethodPatternDemo
{
    // Abstract class defining the template method
    public abstract class Beverage
    {
        // Template method
        public void PrepareBeverage()
        {
            BoilWater();
            Brew();
            PourInCup();
            if (CustomerWantsCondiments())
            {
                AddCondiments();
            }
        }

        protected abstract void Brew();
        protected abstract void AddCondiments();

        private void BoilWater()
        {
            Console.WriteLine("Boiling water");
        }

        private void PourInCup()
        {
            Console.WriteLine("Pouring into cup");
        }

        // Hook method
        protected virtual bool CustomerWantsCondiments()
        {
            return true;
        }
    }

    // Concrete class implementing the template
    public class Tea : Beverage
    {
        protected override void Brew()
        {
            Console.WriteLine("Steeping the tea");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine("Adding lemon");
        }
    }

    // Another concrete class implementing the template
    public class Coffee : Beverage
    {
        protected override void Brew()
        {
            Console.WriteLine("Dripping coffee through filter");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine("Adding sugar and milk");
        }

        // Overriding the hook method
        protected override bool CustomerWantsCondiments()
        {
            Console.Write("Would you like milk and sugar with your coffee (y/n)? ");
            string answer = Console.ReadLine();
            return answer.ToLower().StartsWith("y");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Template Method Pattern Demo: Beverage Preparation");
            Console.WriteLine("=================================================");

            Console.WriteLine("\nPreparing tea...");
            Beverage tea = new Tea();
            tea.PrepareBeverage();

            Console.WriteLine("\nPreparing coffee...");
            Beverage coffee = new Coffee();
            coffee.PrepareBeverage();

            Console.ReadLine();
        }
    }
}