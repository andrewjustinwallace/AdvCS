﻿/*
 * This program demonstrates the Rule Pattern in C#, which allows for flexible and
 * composable business rules. It implements a product filtering system that can
 * combine multiple rules using logical operators.
 * 
 * Key concepts demonstrated:
 * 1. Generic Rule interface (IRule<T>)
 * 2. Concrete rule implementations
 * 3. Composite rules (AND/OR combinations)
 * 4. Rule engine for applying rules to collections
 * 5. Rule chaining and complex rule composition
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each rule class handles one specific validation logic
 *    - RuleEngine focuses solely on applying rules
 *    - Product class manages only product data
 *    - Composite rules handle only rule combination logic
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New rules can be added without modifying existing ones
 *    - Rule combinations can be extended without changing base rules
 *    - RuleEngine works with any rule implementation
 *    - New composite rules can be added without affecting others
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All rules follow IRule<T> contract
 *    - Composite rules can be used anywhere single rules are expected
 *    - Rules maintain consistent behavior when combined
 *    - Rule substitution preserves system behavior
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - IRule<T> interface defines minimal required method
 *    - Rules implement only necessary functionality
 *    - Clients depend only on rules they need
 *    - Rule combinations don't force unnecessary dependencies
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - High-level RuleEngine depends on IRule<T> abstraction
 *    - Rule implementations depend on abstractions
 *    - Rule composition works with abstractions
 *    - Concrete rules are injected at runtime
 * 
 * Pattern benefits:
 * - Encapsulates business rules in separate classes
 * - Enables rule composition and reuse
 * - Provides flexible rule combinations
 * - Maintains single responsibility principle
 * - Easily extendable with new rules
 */

using System;
using System.Collections.Generic;

namespace RulePatternDemo
{
    // Product class
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public Product(string name, decimal price, string category)
        {
            Name = name;
            Price = price;
            Category = category;
        }
    }

    // Rule interface
    public interface IRule<T>
    {
        bool IsSatisfied(T item);
    }

    // Concrete Rules
    public class PriceRule : IRule<Product>
    {
        private decimal _minPrice;
        private decimal _maxPrice;

        public PriceRule(decimal minPrice, decimal maxPrice)
        {
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public bool IsSatisfied(Product item)
        {
            return item.Price >= _minPrice && item.Price <= _maxPrice;
        }
    }

    public class CategoryRule : IRule<Product>
    {
        private string _category;

        public CategoryRule(string category)
        {
            _category = category;
        }

        public bool IsSatisfied(Product item)
        {
            return item.Category == _category;
        }
    }

    // Rule Engine
    public class RuleEngine<T>
    {
        public List<T> Filter(IEnumerable<T> items, IRule<T> rule)
        {
            List<T> result = new List<T>();
            foreach (var item in items)
            {
                if (rule.IsSatisfied(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }

    public class NameRule : IRule<Product>
    {
        private string _nameContains;

        public NameRule(string nameContains)
        {
            _nameContains = nameContains.ToLower();
        }

        public bool IsSatisfied(Product item)
        {
            return item.Name.ToLower().Contains(_nameContains);
        }
    }

    // Composite Rules
    public class AndRule<T> : IRule<T>
    {
        private readonly List<IRule<T>> _rules = new List<IRule<T>>();

        public AndRule(params IRule<T>[] rules)
        {
            _rules.AddRange(rules);
        }

        public bool IsSatisfied(T item)
        {
            foreach (var rule in _rules)
            {
                if (!rule.IsSatisfied(item))
                    return false;
            }
            return true;
        }
    }

    public class OrRule<T> : IRule<T>
    {
        private readonly List<IRule<T>> _rules = new List<IRule<T>>();

        public OrRule(params IRule<T>[] rules)
        {
            _rules.AddRange(rules);
        }

        public bool IsSatisfied(T item)
        {
            foreach (var rule in _rules)
            {
                if (rule.IsSatisfied(item))
                    return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>
            {
                new Product("Gaming Laptop", 1500, "Electronics"),
                new Product("Smartphone", 800, "Electronics"),
                new Product("Programming Book", 50, "Books"),
                new Product("Wireless Headphones", 200, "Electronics"),
                new Product("Smart Watch", 300, "Electronics"),
                new Product("T-shirt", 30, "Clothing")
            };

            // Create individual rules
            IRule<Product> expensiveRule = new PriceRule(500, decimal.MaxValue);
            IRule<Product> moderatePriceRule = new PriceRule(100, 500);
            IRule<Product> electronicsRule = new CategoryRule("Electronics");
            IRule<Product> smartRule = new NameRule("smart");

            // Combine rules using AndRule and OrRule
            IRule<Product> expensiveElectronics = new AndRule<Product>(expensiveRule, electronicsRule);
            IRule<Product> moderateElectronics = new AndRule<Product>(moderatePriceRule, electronicsRule);
            IRule<Product> smartProducts = new AndRule<Product>(electronicsRule, smartRule);

            IRule<Product> complexRule = new OrRule<Product>(
                expensiveElectronics,
                new AndRule<Product>(moderateElectronics, smartProducts)
            );

            // Create and use rule engine
            RuleEngine<Product> engine = new RuleEngine<Product>();
            List<Product> filteredProducts = engine.Filter(products, complexRule);

            // Display results
            Console.WriteLine("Filtered Products (Expensive Electronics OR (Moderate-priced Electronics AND Smart)):");
            foreach (var product in filteredProducts)
            {
                Console.WriteLine($"{product.Name} - ${product.Price} - {product.Category}");
            }

            Console.ReadLine();
        }
    }
}