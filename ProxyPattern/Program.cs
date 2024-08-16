
//This pattern allows you to add these security measures without changing the RealSubject or the Client code, adhering to the Open/Closed principle of SOLID.

using System;

public interface ISubject
{
    void Request();
}

public class RealSubject : ISubject
{
    public void Request()
    {
        Console.WriteLine("RealSubject: Handling Request.");
    }
}

public class Proxy : ISubject
{
    private RealSubject? _realSubject;
    private string _userRole;

    public Proxy(string userRole)
    {
        _userRole = userRole;
    }

    public void Request()
    {
        if (CheckAccess())
        {
            if (_realSubject == null)
            {
                Console.WriteLine("Proxy: Creating RealSubject instance.");
                _realSubject = new RealSubject();
            }

            Console.WriteLine("Proxy: Logging before request.");
            _realSubject.Request();
            Console.WriteLine("Proxy: Logging after request.");
        }
        else
        {
            Console.WriteLine("Proxy: Access denied.");
        }
    }

    private bool CheckAccess()
    {
        Console.WriteLine("Proxy: Checking access prior to firing a real request.");
        return _userRole == "Admin";
    }
}

public class Client
{
    public static void Main()
    {
        Console.WriteLine("Client: Attempting to access with Admin role:");
        Proxy adminProxy = new Proxy("Admin");
        adminProxy.Request();

        Console.WriteLine("\nClient: Attempting to access with User role:");
        Proxy userProxy = new Proxy("User");
        userProxy.Request();
    }
}