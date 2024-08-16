// The interface that the client expects to work with
public interface ITarget
{
    string GetRequest();
}

// The interface that needs to be adapted
public class Adaptee
{
    public string GetSpecificRequest()
    {
        return "Specific request.";
    }
}

// The Adapter class
public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }

    public string GetRequest()
    {
        return $"Adapter: {_adaptee.GetSpecificRequest()}";
    }
}

// Client code
public class Client
{
    public void MakeRequest(ITarget target)
    {
        Console.WriteLine(target.GetRequest());
    }
}

// Usage
class Program
{
    static void Main(string[] args)
    {
        Adaptee adaptee = new Adaptee();
        ITarget target = new Adapter(adaptee);

        Client client = new Client();
        client.MakeRequest(target);
    }
}