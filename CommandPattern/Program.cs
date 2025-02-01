/*
 * This program demonstrates the Command design pattern using a smart home lighting system example.
 * The Command pattern encapsulates a request as an object, allowing you to parameterize clients
 * with different requests, queue or log requests, and support undoable operations.
 * 
 * Key concepts demonstrated:
 * 1. Command Interface (ICommand) with Execute and Undo operations
 * 2. Concrete Commands (LightOnCommand, LightOffCommand)
 * 3. Receiver (Light) that performs the actual operations
 * 4. Invoker (RemoteControl) that handles command execution and undo stack
 * 5. Command history implementation using Stack<T>
 * 6. Undo functionality implementation
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each command class has single responsibility of encapsulating one operation
 *    - Light class handles only lighting operations
 *    - RemoteControl focuses solely on command execution and history
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New commands can be added without modifying existing commands
 *    - RemoteControl works with any command without modification
 *    - Command history works with any command that implements ICommand
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All commands can be used interchangeably through ICommand interface
 *    - RemoteControl works with any concrete command implementation
 *    - Command history treats all commands uniformly
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - ICommand interface defines minimal required methods (Execute and Undo)
 *    - Commands implement only the methods they need
 *    - Clients depend only on the commands they use
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - High-level RemoteControl depends on ICommand abstraction
 *    - Command implementations depend on abstractions
 *    - Concrete commands are injected into RemoteControl
 * 
 * Pattern benefits:
 * - Decouples object making request from objects that handle the request
 * - Enables command queueing and command history
 * - Supports undo operations
 * - Allows adding new commands without changing existing code
 * - Promotes Single Responsibility Principle
 */

using System;
using System.Collections.Generic;

namespace CommandPatternDemo
{
    // Command interface
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Receiver class
    public class Light
    {
        private bool _isOn = false;
        private string _location;

        public Light(string location)
        {
            _location = location;
        }

        public void TurnOn()
        {
            _isOn = true;
            Console.WriteLine($"{_location} light is now ON");
        }

        public void TurnOff()
        {
            _isOn = false;
            Console.WriteLine($"{_location} light is now OFF");
        }
    }

    // Concrete Command for turning on the light
    public class LightOnCommand : ICommand
    {
        private Light _light;

        public LightOnCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOn();
        }

        public void Undo()
        {
            _light.TurnOff();
        }
    }

    // Concrete Command for turning off the light
    public class LightOffCommand : ICommand
    {
        private Light _light;

        public LightOffCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOff();
        }

        public void Undo()
        {
            _light.TurnOn();
        }
    }

    // Invoker
    public class RemoteControl
    {
        private ICommand _command;
        private Stack<ICommand> _commandHistory = new Stack<ICommand>();

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void PressButton()
        {
            _command.Execute();
            _commandHistory.Push(_command);
        }

        public void PressUndo()
        {
            if (_commandHistory.Count > 0)
            {
                ICommand lastCommand = _commandHistory.Pop();
                lastCommand.Undo();
            }
            else
            {
                Console.WriteLine("No commands to undo");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Command Pattern Demo: Smart Home Lighting");
            Console.WriteLine("=========================================");

            // Create receivers
            Light livingRoomLight = new Light("Living Room");
            Light kitchenLight = new Light("Kitchen");

            // Create commands
            ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
            ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
            ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
            ICommand kitchenLightOff = new LightOffCommand(kitchenLight);

            // Create invoker
            RemoteControl remote = new RemoteControl();

            // Use the remote
            remote.SetCommand(livingRoomLightOn);
            remote.PressButton();

            remote.SetCommand(kitchenLightOn);
            remote.PressButton();

            remote.SetCommand(livingRoomLightOff);
            remote.PressButton();

            Console.WriteLine("\nUndo last action");
            remote.PressUndo();

            Console.WriteLine("\nUndo another action");
            remote.PressUndo();

            Console.ReadLine();
        }
    }
}