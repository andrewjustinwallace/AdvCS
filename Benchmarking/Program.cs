/*
 * This program demonstrates the use of BenchmarkDotNet to perform performance benchmarking in C#.
 * It specifically compares different string concatenation methods to analyze their performance characteristics.
 * 
 * Key concepts demonstrated:
 * 1. BenchmarkDotNet framework usage
 * 2. Memory diagnostics with [MemoryDiagnoser]
 * 3. Benchmark configuration
 * 4. Method-level benchmarking with [Benchmark] attribute
 * 5. Performance comparison of different string concatenation approaches
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each benchmark method has a single responsibility of testing one concatenation approach
 *    - The benchmark class focuses solely on string concatenation performance testing
 *    - Configuration setup is separated from benchmark implementation
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New benchmarks can be added by creating new methods without modifying existing ones
 *    - Configuration can be extended without changing benchmark implementation
 *    - Attribute-based approach allows for extension without modification
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All benchmark methods follow the same contract defined by BenchmarkDotNet
 *    - Different benchmark configurations can be substituted without affecting the tests
 *    - Results remain consistent with the BenchmarkDotNet contract
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Each benchmark method implements only the necessary operations
 *    - Configuration uses specific interfaces for different aspects (logging, validation)
 *    - Diagnostics are applied selectively using attributes
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Benchmark framework depends on abstractions (attributes, interfaces)
 *    - Configuration is built using abstract providers and validators
 *    - Logger and validator implementations are injected through configuration
 * 
 * The benchmark compares:
 * - Plus operator concatenation ("+" operator)
 * - String.Concat method
 * 
 * Results include:
 * - Execution time
 * - Memory allocation
 * - GC collection statistics
 */

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

[MemoryDiagnoser]
public class StringConcatBenchmark
{
    [Benchmark]
    public string PlusOperator() => "Hello" + " " + "World";

    [Benchmark]
    public string StringConcat() => string.Concat("Hello", " ", "World");
}

class Program
{
    static void Main(string[] args)
    {
        var config = new ManualConfig()
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddValidator(JitOptimizationsValidator.DontFailOnError)
        .AddLogger(ConsoleLogger.Default)
        .AddColumnProvider(DefaultColumnProviders.Instance);

        BenchmarkRunner.Run<StringConcatBenchmark>(config);
    }
}