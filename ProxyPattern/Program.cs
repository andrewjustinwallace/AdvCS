/*
 * This program demonstrates the Proxy design pattern, which provides a surrogate or placeholder
 * for another object to control access to it. The example implements a security proxy that
 * controls access to a resource based on user roles.
 * 
 * Key concepts demonstrated:
 * 1. Common Interface (ISubject) - defines the interface for RealSubject and Proxy
 * 2. RealSubject - the actual object that the proxy represents
 * 3. Proxy - controls access to RealSubject and can handle:
 *    - Access control (security proxy)
 *    - Lazy initialization
 *    - Logging and monitoring
 * 
 * Pattern benefits:
 * - Security: Controls access to sensitive objects
 * - Performance: Supports lazy loading of expensive objects
 * - Monitoring: Enables logging and access tracking
 * - Single Responsibility: Separates access control from business logic
 * 
 * The example shows:
 * - Role-based access control
 * - Lazy initialization of RealSubject
 * - Request logging before and after execution
 */

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