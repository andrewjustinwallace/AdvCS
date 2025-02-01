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

    // Rest of the code...
}