/*
 * This program demonstrates two implementations of the Observer pattern in C#:
 * 1. Traditional event-based implementation using C#'s built-in event system
 * 2. Modern implementation using IObservable<T> and IObserver<T> interfaces
 * 
 * The example uses a weather station scenario where:
 * - Weather stations (Subjects) publish weather data
 * - Display units (Observers) subscribe to receive updates
 * 
 * Key concepts demonstrated:
 * 1. Traditional Event Pattern:
 *    - EventHandler<T> delegate
 *    - Event declaration and raising
 *    - Event subscription and handling
 * 
 * 2. IObservable Pattern:
 *    - IObservable<T> interface implementation
 *    - IObserver<T> interface implementation
 *    - Subscription management with IDisposable
 *    - Observer notification (OnNext, OnError, OnCompleted)
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - WeatherData class solely manages weather measurements
 *    - WeatherStation focuses only on publishing updates
 *    - Display classes handle only their specific display logic
 *    - Unsubscriber handles only subscription management
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New observers can be added without modifying subjects
 *    - New display types can be created without changing weather stations
 *    - Weather data can be extended without affecting observers
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Any IObserver<WeatherData> can be used with ObservableWeatherStation
 *    - Different weather station implementations can be substituted
 *    - Display implementations are interchangeable within their types
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - IObservable interface provides focused subscription method
 *    - IObserver interface defines minimal required methods
 *    - Event handlers implement only necessary functionality
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - High-level modules depend on IObservable/IObserver abstractions
 *    - Event handlers depend on EventHandler delegate type
 *    - Concrete implementations depend on abstractions
 * 
 * Pattern benefits:
 * - Loose coupling between subjects and observers
 * - Support for multiple observers
 * - Dynamic subscription/unsubscription
 * - Clean separation of concerns
 */

using System;
using System.Collections.Generic;

// Weather data
public class WeatherData
{
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float Pressure { get; set; }
}

// Traditional Event-based Subject
public class WeatherStation
{
    private WeatherData _weatherData = new WeatherData();

    // Define the event
    public event EventHandler<WeatherData>? WeatherChanged;

    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        _weatherData.Temperature = temperature;
        _weatherData.Humidity = humidity;
        _weatherData.Pressure = pressure;
        OnWeatherChanged();
    }

    protected virtual void OnWeatherChanged()
    {
        WeatherChanged?.Invoke(this, _weatherData);
    }
}

// Traditional Event-based Observer
public class CurrentConditionsDisplay
{
    public CurrentConditionsDisplay(WeatherStation weatherStation)
    {
        weatherStation.WeatherChanged += OnWeatherChanged;
    }

    private void OnWeatherChanged(object? sender, WeatherData e)
    {
        Console.WriteLine($"Current conditions: {e.Temperature}F degrees and {e.Humidity}% humidity");
    }
}

// IObservable-based Subject
public class ObservableWeatherStation : IObservable<WeatherData>
{
    private WeatherData _weatherData = new WeatherData();
    private List<IObserver<WeatherData>> _observers = new List<IObserver<WeatherData>>();

    public IDisposable Subscribe(IObserver<WeatherData> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        _weatherData.Temperature = temperature;
        _weatherData.Humidity = humidity;
        _weatherData.Pressure = pressure;
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(_weatherData);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<WeatherData>> _observers;
        private IObserver<WeatherData> _observer;

        public Unsubscriber(List<IObserver<WeatherData>> observers, IObserver<WeatherData> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}

// IObserver-based Observer
public class StatisticsDisplay : IObserver<WeatherData>
{
    private float _maxTemp = float.MinValue;
    private float _minTemp = float.MaxValue;
    private float _tempSum = 0.0f;
    private int _numReadings = 0;

    public void OnCompleted()
    {
        Console.WriteLine("Weather station has completed transmitting data");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"Error occurred: {error.Message}");
    }

    public void OnNext(WeatherData value)
    {
        _tempSum += value.Temperature;
        _numReadings++;

        if (value.Temperature > _maxTemp)
            _maxTemp = value.Temperature;
        if (value.Temperature < _minTemp)
            _minTemp = value.Temperature;

        Console.WriteLine($"Avg/Max/Min temperature = {_tempSum / _numReadings}/{_maxTemp}/{_minTemp}");
    }
}

// Client code
public class WeatherApp
{
    public static void Main()
    {
        Console.WriteLine("Using traditional events:");
        var weatherStation = new WeatherStation();
        var currentDisplay = new CurrentConditionsDisplay(weatherStation);

        weatherStation.SetMeasurements(80, 65, 30.4f);
        weatherStation.SetMeasurements(82, 70, 29.2f);

        Console.WriteLine("\nUsing IObservable:");
        var observableWeatherStation = new ObservableWeatherStation();
        var statisticsDisplay = new StatisticsDisplay();

        using (observableWeatherStation.Subscribe(statisticsDisplay))
        {
            observableWeatherStation.SetMeasurements(80, 65, 30.4f);
            observableWeatherStation.SetMeasurements(82, 70, 29.2f);
        }
    }
}