using System;
using System.Collections.Generic;

namespace ObserverPatternDemo
{
    // Subject interface
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers();
    }

    // Observer interface
    public interface IObserver
    {
        void Update(string message);
    }

    // Concrete Subject
    public class NewsAgency : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _latestNews;

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_latestNews);
            }
        }

        public void SetNews(string news)
        {
            _latestNews = news;
            NotifyObservers();
        }
    }

    // Concrete Observer
    public class NewsSubscriber : IObserver
    {
        private string _name;

        public NewsSubscriber(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{_name} received news: {message}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a news agency
            NewsAgency agency = new NewsAgency();

            // Create subscribers
            NewsSubscriber john = new NewsSubscriber("John");
            NewsSubscriber jane = new NewsSubscriber("Jane");
            NewsSubscriber bob = new NewsSubscriber("Bob");

            // Register subscribers
            agency.RegisterObserver(john);
            agency.RegisterObserver(jane);
            agency.RegisterObserver(bob);

            // Set news, which will notify all subscribers
            agency.SetNews("Breaking: Observer Pattern Implemented Successfully!");

            // Unregister one subscriber
            agency.RemoveObserver(jane);

            // Set news again
            agency.SetNews("Update: Jane Has Unsubscribed!");

            Console.ReadLine();
        }
    }
}