using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

public interface ICalculator
{
    double Add(double a, double b);
    double Subtract(double a, double b);
    double Multiply(double a, double b);
    double Divide(double a, double b);
}

public class BasicCalculator : ICalculator
{
    public double Add(double a, double b) => a + b;
    public double Subtract(double a, double b) => a - b;
    public double Multiply(double a, double b) => a * b;
    public double Divide(double a, double b) => a / b;
}

public class PreciseCalculator : ICalculator
{
    private readonly int _precision;

    public PreciseCalculator(int precision)
    {
        _precision = precision;
    }

    public double Add(double a, double b) => Math.Round(a + b, _precision);
    public double Subtract(double a, double b) => Math.Round(a - b, _precision);
    public double Multiply(double a, double b) => Math.Round(a * b, _precision);
    public double Divide(double a, double b) => Math.Round(a / b, _precision);
}

[TestClass]
public class CalculatorTests
{
    public static IEnumerable<object[]> GetCalculators()
    {
        yield return new object[] { new BasicCalculator(), 0 };
        yield return new object[] { new PreciseCalculator(2), 2 };
        yield return new object[] { new PreciseCalculator(4), 4 };
    }

    [DataTestMethod]
    [DynamicData(nameof(GetCalculators), DynamicDataSourceType.Method)]
    public void TestAddition(ICalculator calculator, int precision)
    {
        double result = calculator.Add(2, 2);
        Assert.AreEqual(4, result, Math.Pow(10, -precision));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetCalculators), DynamicDataSourceType.Method)]
    public void TestSubtraction(ICalculator calculator, int precision)
    {
        double result = calculator.Subtract(2, 2);
        Assert.AreEqual(0, result, Math.Pow(10, -precision));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetCalculators), DynamicDataSourceType.Method)]
    public void TestMultiplication(ICalculator calculator, int precision)
    {
        double result = calculator.Multiply(2, 2);
        Assert.AreEqual(4, result, Math.Pow(10, -precision));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetCalculators), DynamicDataSourceType.Method)]
    public void TestDivision(ICalculator calculator, int precision)
    {
        double result = calculator.Divide(2, 2);
        Assert.AreEqual(1, result, Math.Pow(10, -precision));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetCalculators), DynamicDataSourceType.Method)]
    public void TestPrecision(ICalculator calculator, int expectedPrecision)
    {
        if (calculator is PreciseCalculator)
        {
            double result = calculator.Divide(1, 3);
            int actualPrecision = BitConverter.GetBytes(decimal.GetBits((decimal)result)[3])[2];
            Assert.AreEqual(expectedPrecision, actualPrecision);
        }
        else
        {
            Assert.AreEqual(0, expectedPrecision);
        }
    }
}