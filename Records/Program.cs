/*
 * This program demonstrates the use of C# Records, a feature introduced in C# 9.0.
 * It implements a simple order management system to showcase how records can be used
 * for creating immutable data models with built-in value equality.
 * 
 * Key concepts demonstrated:
 * 1. Record declarations with positional and standard syntax
 * 2. Immutable collections (ImmutableList<T>)
 * 3. Non-destructive mutation with 'with' expressions
 * 4. Value-based equality
 * 5. Built-in toString implementation
 * 6. Pattern matching with records
 * 
 * The example includes:
 * - Product, OrderItem, Order, and OrderStatus records
 * - OrderService class demonstrating record manipulation
 * - Exception handling for business logic
 * - LINQ operations with records
 */

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

// Product record
public record Product(string Id, string Name, decimal Price);

// Order item record
public record OrderItem(Product Product, int Quantity)
{
    public decimal TotalPrice => Product.Price * Quantity;
}

// Order record
public record Order(string Id, DateTime OrderDate, string CustomerId, ImmutableList<OrderItem> Items)
{
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    public int TotalItems => Items.Sum(item => item.Quantity);
}

// Order status record
public record OrderStatus(string OrderId, string Status, DateTime LastUpdated);

// Order service
public class OrderService
{
    private ImmutableList<Order> _orders = ImmutableList<Order>.Empty;
    private ImmutableList<OrderStatus> _orderStatuses = ImmutableList<OrderStatus>.Empty;

    public Order CreateOrder(string customerId, IEnumerable<OrderItem> items)
    {
        var order = new Order(
            Id: Guid.NewGuid().ToString(),
            OrderDate: DateTime.UtcNow,
            CustomerId: customerId,
            Items: items.ToImmutableList()
        );

        _orders = _orders.Add(order);
        _orderStatuses = _orderStatuses.Add(new OrderStatus(order.Id, "Pending", DateTime.UtcNow));

        return order;
    }

    public Order UpdateOrderStatus(string orderId, string newStatus)
    {
        var existingOrder = _orders.SingleOrDefault(o => o.Id == orderId)
            ?? throw new ArgumentException("Order not found", nameof(orderId));

        var existingStatus = _orderStatuses.SingleOrDefault(os => os.OrderId == orderId)
            ?? throw new InvalidOperationException("Order status not found");

        var updatedStatus = existingStatus with
        {
            Status = newStatus,
            LastUpdated = DateTime.UtcNow
        };

        _orderStatuses = _orderStatuses.Replace(existingStatus, updatedStatus);

        return existingOrder;
    }

    public Order AddItemToOrder(string orderId, OrderItem newItem)
    {
        var existingOrder = _orders.SingleOrDefault(o => o.Id == orderId)
            ?? throw new ArgumentException("Order not found", nameof(orderId));

        var updatedItems = existingOrder.Items.Add(newItem);
        var updatedOrder = existingOrder with { Items = updatedItems };

        _orders = _orders.Replace(existingOrder, updatedOrder);

        return updatedOrder;
    }

    public IEnumerable<Order> GetOrdersByCustomer(string customerId)
    {
        return _orders.Where(o => o.CustomerId == customerId);
    }

    public OrderStatus GetOrderStatus(string orderId)
    {
        return _orderStatuses.SingleOrDefault(os => os.OrderId == orderId)
            ?? throw new ArgumentException("Order status not found", nameof(orderId));
    }
}

class Program
{
    static void Main()
    {
        var orderService = new OrderService();

        // Create some products
        var product1 = new Product("P1", "Laptop", 999.99m);
        var product2 = new Product("P2", "Mouse", 24.99m);

        try
        {
            // Create an order
            var order = orderService.CreateOrder("C001", new[]
            {
                new OrderItem(product1, 1),
                new OrderItem(product2, 2)
            });

            Console.WriteLine($"Order created: {order.Id}, Total: ${order.TotalAmount}");

            // Update order status
            orderService.UpdateOrderStatus(order.Id, "Processing");
            var status = orderService.GetOrderStatus(order.Id);
            Console.WriteLine($"Order status: {status.Status}, Last updated: {status.LastUpdated}");

            // Add item to order
            var updatedOrder = orderService.AddItemToOrder(order.Id, new OrderItem(product2, 1));
            Console.WriteLine($"Updated order total: ${updatedOrder.TotalAmount}, Total items: {updatedOrder.TotalItems}");

            // Get orders by customer
            var customerOrders = orderService.GetOrdersByCustomer("C001");
            Console.WriteLine($"Customer C001 has {customerOrders.Count()} orders");

            // Attempt to update a non-existent order (to demonstrate exception handling)
            orderService.UpdateOrderStatus("non-existent-id", "Shipped");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Operation Error: {ex.Message}");
        }
    }
}