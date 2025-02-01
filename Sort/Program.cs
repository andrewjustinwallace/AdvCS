/*
 * This program demonstrates generic sorting in C# using the bubble sort algorithm.
 * It shows how to implement sorting for both built-in types and custom classes
 * using the IComparable<T> interface.
 * 
 * Key concepts demonstrated:
 * 1. Generic classes and constraints
 * 2. IComparable<T> interface implementation
 * 3. Bubble sort algorithm
 * 4. Type-safe comparisons
 * 5. Custom object sorting
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - SortArray class handles only sorting logic
 *    - Employee class manages only employee data
 *    - CompareTo method focuses solely on comparison logic
 *    - Swap method handles only element exchange
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New sortable types can be added without modifying sort logic
 *    - Employee comparison can be changed without affecting sort algorithm
 *    - Sort class works with any IComparable<T> implementation
 *    - New sorting algorithms can be added without changing existing code
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Any IComparable<T> type can be sorted
 *    - Sort behavior remains consistent across types
 *    - Employee implements comparison contract correctly
 *    - Generic constraint ensures type safety
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - IComparable<T> provides focused comparison method
 *    - Types implement only necessary comparison logic
 *    - Sort algorithm depends only on comparison capability
 *    - Employee exposes minimal required interface
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Sort logic depends on IComparable<T> abstraction
 *    - Implementation details are passed through generics
 *    - Concrete types implement comparison interface
 *    - Algorithm works with interface, not concrete types
 * 
 * Examples include:
 * - Sorting integers (commented out)
 * - Sorting strings (commented out)
 * - Sorting custom Employee objects by name
 * - Generic swap implementation
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace GenericBubbleSortApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //  int[] arr = new int[] { 2, 1, 4, 3 };

            //string[] arr = new string[] { "Bob", "Henry", "Andy", "Greg" };

            Employee[] arr = new Employee[] { new Employee { Id = 4, Name = "John" },
                                               new Employee { Id = 2, Name = "Bob" },
                                               new Employee { Id = 3, Name = "Greg" },
                                               new Employee { Id = 1, Name = "Tom" }};

            SortArray<Employee> sortArray = new SortArray<Employee>();
            //SortArray<int> sortArray = new SortArray<int>();
            // SortArray<string> sortArray = new SortArray<string>();

            sortArray.BubbleSort(arr);

            foreach (var item in arr)
                Console.WriteLine(item);

            Console.ReadKey();

        }
    }

    public class Employee : IComparable<Employee>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompareTo([AllowNull] Employee other)
        {
            return this.Name.CompareTo(other.Name);
        }

        /*  public int CompareTo(object obj)
        {
            return this.Id.CompareTo(((Employee)obj).Id);
        }*/

        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }

    public class SortArray<T> where T : IComparable<T>
    {
        public void BubbleSort(T[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        Swap(arr, j);
                    }
        }

        private void Swap(T[] arr, int j)
        {
            T temp = arr[j];
            arr[j] = arr[j + 1];
            arr[j + 1] = temp;
        }

    }
}