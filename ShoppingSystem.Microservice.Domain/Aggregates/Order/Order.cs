using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Aggregates.Order;

/// <summary>
/// Represents the Order aggregate root.
/// Responsible for managing order information, order items,
/// payments, pricing calculations, and order status changes.
/// </summary>
public class Order : AggregateRoot
{
    // Private constructor required by ORM frameworks for entity materialization.
    private Order()
    {
    }

    /// <summary>
    /// Creates a new order instance with initial pending status and zero total price.
    /// </summary>
    /// <param name="userId">The identifier of the user who created the order.</param>
    /// <param name="orderNumber">The unique order number.</param>
    private Order(
        Guid userId,
        OrderNumber orderNumber)
    {
        UserId = userId;
        OrderNumber = orderNumber;
        Status = OrderStatus.Pending;
        TotalPrice = Money.Create(0, "USD");
    }


    /// <summary>
    /// Creates a new Order aggregate.
    /// </summary>
    /// <param name="userId">The identifier of the user creating the order.</param>
    /// <param name="orderNumber">The order number value object.</param>
    /// <returns>A new Order instance.</returns>
    public static Order Create(
        Guid userId,
        OrderNumber orderNumber)
    {
        return new Order(
            userId,
            orderNumber
        );
    }


    /// <summary>
    /// Updates the current status of the order.
    /// </summary>
    /// <param name="status">The new order status.</param>
    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }


    /// <summary>
    /// Gets the identifier of the user who owns this order.
    /// </summary>
    public Guid UserId { get; private set; }


    /// <summary>
    /// Gets the unique order number.
    /// </summary>
    public OrderNumber OrderNumber { get; private set; }


    /// <summary>
    /// Gets the total price of all items in the order.
    /// </summary>
    public Money TotalPrice { get; private set; } = Money.Create(0, "USD");


    /// <summary>
    /// Gets the current status of the order.
    /// </summary>
    public OrderStatus Status { get; private set; }


    // Collection of payments associated with this order.
    private readonly List<Payment.Payment> _payments;

    /// <summary>
    /// Provides read-only access to order payments.
    /// </summary>
    public IReadOnlyCollection<Payment.Payment> Payments => _payments;


    // Internal collection used to manage order items.
    private readonly List<OrderItem> OrderItems = new();


    /// <summary>
    /// Provides read-only access to items included in the order.
    /// </summary>
    public IReadOnlyCollection<OrderItem> Items =>
        OrderItems.AsReadOnly();


    /// <summary>
    /// Adds a new product item to the order and updates the total price.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantity">The requested quantity.</param>
    public void AddItem(
        Guid productId,
        ProductName productName,
        Money price,
        Quantity quantity)
    {
        // Creates a new order item using the provided product details.
        var item = OrderItem.Create(
            productId,
            productName,
            price,
            quantity
        );

        // Adds the created item to the order items collection.
        OrderItems.Add(item);


        // Recalculates the order total price after adding the new item.
        TotalPrice = Money.Create(
            TotalPrice.Amount + item.Price.Amount * quantity.Value,
            TotalPrice.Currency
        );
    }
}