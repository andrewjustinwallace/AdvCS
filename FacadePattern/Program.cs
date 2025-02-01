/*
 * This program demonstrates the Facade design pattern using a home theater system example.
 * The Facade pattern provides a unified interface to a set of interfaces in a subsystem,
 * making the subsystem easier to use by defining a higher-level interface.
 * 
 * Key concepts demonstrated:
 * 1. Complex subsystem classes (Amplifier, DvdPlayer, Projector)
 * 2. Facade class (HomeTheaterFacade) providing simplified interface
 * 3. Client code using the facade instead of subsystem classes directly
 * 4. Dependency injection in facade constructor
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each subsystem class has one specific responsibility (Amplifier handles audio, Projector handles video)
 *    - Facade handles the single responsibility of coordinating these components
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New functionality can be added by creating new facade methods without modifying existing ones
 *    - Additional subsystem components can be added without changing the facade interface
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Different implementations of subsystem components can be substituted (e.g., different projector types)
 *    - Facade works with abstractions, allowing component substitution
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Subsystem components expose focused interfaces specific to their functionality
 *    - Facade provides simplified interface tailored to client needs
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Facade depends on abstractions injected through constructor
 *    - High-level and low-level modules depend on abstractions
 * 
 * Pattern benefits:
 * - Simplifies complex subsystem interaction
 * - Reduces coupling between client and subsystem
 * - Provides unified interface to set of interfaces
 * - Promotes loose coupling
 * - Hides subsystem complexity
 */

using System;

// Complex subsystem classes
public class Amplifier
{
    public void On() => Console.WriteLine("Amplifier is on");
    public void SetVolume(int level) => Console.WriteLine($"Amplifier volume set to {level}");
    public void Off() => Console.WriteLine("Amplifier is off");
}

public class DvdPlayer
{
    public void On() => Console.WriteLine("DVD Player is on");
    public void Play(string movie) => Console.WriteLine($"DVD Player is playing: {movie}");
    public void Stop() => Console.WriteLine("DVD Player stopped");
    public void Off() => Console.WriteLine("DVD Player is off");
}

public class Projector
{
    public void On() => Console.WriteLine("Projector is on");
    public void SetInput(DvdPlayer dvd) => Console.WriteLine("Projector input set to DVD Player");
    public void Off() => Console.WriteLine("Projector is off");
}

// Facade
public class HomeTheaterFacade
{
    private readonly Amplifier _amplifier;
    private readonly DvdPlayer _dvdPlayer;
    private readonly Projector _projector;

    public HomeTheaterFacade(Amplifier amplifier, DvdPlayer dvdPlayer, Projector projector)
    {
        _amplifier = amplifier;
        _dvdPlayer = dvdPlayer;
        _projector = projector;
    }

    public void WatchMovie(string movie)
    {
        Console.WriteLine("Get ready to watch a movie...");
        _amplifier.On();
        _amplifier.SetVolume(5);
        _projector.On();
        _projector.SetInput(_dvdPlayer);
        _dvdPlayer.On();
        _dvdPlayer.Play(movie);
    }

    public void EndMovie()
    {
        Console.WriteLine("Shutting down the home theater...");
        _dvdPlayer.Stop();
        _dvdPlayer.Off();
        _projector.Off();
        _amplifier.Off();
    }
}

// Client code
public class Client
{
    public static void Main()
    {
        var amplifier = new Amplifier();
        var dvdPlayer = new DvdPlayer();
        var projector = new Projector();

        var homeTheater = new HomeTheaterFacade(amplifier, dvdPlayer, projector);

        homeTheater.WatchMovie("Inception");
        Console.WriteLine();
        homeTheater.EndMovie();
    }
}