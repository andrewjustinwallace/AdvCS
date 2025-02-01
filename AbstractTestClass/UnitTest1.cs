/*
 * This file demonstrates the use of abstract test classes in C# unit testing to implement
 * the Template Method pattern for database testing.
 * 
 * Key concepts demonstrated:
 * 1. Abstract Test Class Pattern - shared test logic in a base class
 * 2. Template Method Pattern - CreateDatabase() as the template method
 * 3. Interface-based design with IDatabase
 * 4. Abstract class implementation with Database base class
 * 5. Multiple concrete implementations (SQL Server and Postgres)
 * 6. MSTest testing framework features - TestInitialize, TestCleanup, TestMethod
 * 
 * The pattern allows:
 * - Common test logic to be shared across different database implementations
 * - Specific test cases for each database type
 * - Consistent test setup and teardown
 * - Easy addition of new database implementations
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

// Classes to be tested
public interface IDatabase
{
    bool IsConnected { get; }
    void Connect();
    void Disconnect();
    string ExecuteQuery(string query);
}

public abstract class Database : IDatabase
{
    public bool IsConnected { get; protected set; }
    public abstract void Connect();
    public abstract void Disconnect();
    public abstract string ExecuteQuery(string query);
}

public class SqlServerDatabase : Database
{
    private readonly string _connectionString;

    public SqlServerDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override void Connect()
    {
        // Simulating connection
        IsConnected = true;
    }

    public override void Disconnect()
    {
        // Simulating disconnection
        IsConnected = false;
    }

    public override string ExecuteQuery(string query)
    {
        return $"SQL Server result for: {query}";
    }
}

public class PostgresDatabase : Database
{
    private readonly string _connectionString;

    public PostgresDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override void Connect()
    {
        // Simulating connection
        IsConnected = true;
    }

    public override void Disconnect()
    {
        // Simulating disconnection
        IsConnected = false;
    }

    public override string ExecuteQuery(string query)
    {
        return $"Postgres result for: {query}";
    }
}

// Abstract Test Class
[TestClass]
public abstract class AbstractDatabaseTests
{
    protected IDatabase _database;

    [TestInitialize]
    public virtual void Setup()
    {
        _database = CreateDatabase();
        _database.Connect();
    }

    [TestCleanup]
    public virtual void Teardown()
    {
        _database.Disconnect();
    }

    protected abstract IDatabase CreateDatabase();

    [TestMethod]
    public void Connect_ShouldEstablishConnection()
    {
        Assert.IsTrue(_database.IsConnected);
    }

    [TestMethod]
    public void Disconnect_ShouldCloseConnection()
    {
        _database.Disconnect();
        Assert.IsFalse(_database.IsConnected);
    }

    [TestMethod]
    public void ExecuteQuery_ShouldReturnResult()
    {
        string result = _database.ExecuteQuery("SELECT * FROM Users");
        Assert.IsFalse(string.IsNullOrEmpty(result));
    }
}

// Concrete Test Classes
[TestClass]
public class SqlServerDatabaseTests : AbstractDatabaseTests
{
    protected override IDatabase CreateDatabase()
    {
        return new SqlServerDatabase("sql_server_connection_string");
    }

    [TestMethod]
    public void ExecuteQuery_ShouldReturnSqlServerSpecificResult()
    {
        string result = _database.ExecuteQuery("SELECT * FROM Users");
        StringAssert.StartsWith(result, "SQL Server result for:");
    }
}

[TestClass]
public class PostgresDatabaseTests : AbstractDatabaseTests
{
    protected override IDatabase CreateDatabase()
    {
        return new PostgresDatabase("postgres_connection_string");
    }

    [TestMethod]
    public void ExecuteQuery_ShouldReturnPostgresSpecificResult()
    {
        string result = _database.ExecuteQuery("SELECT * FROM Users");
        StringAssert.StartsWith(result, "Postgres result for:");
    }
}