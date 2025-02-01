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