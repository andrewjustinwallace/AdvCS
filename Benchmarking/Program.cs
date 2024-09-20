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