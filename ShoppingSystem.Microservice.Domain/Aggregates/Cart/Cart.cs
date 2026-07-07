using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Item;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Aggregates.Cart;

/// <summary>
/// Represents the Cart aggregate root.
/// A cart contains a collection of items associated with a specific customer
/// and manages cart-related business rules and domain events.
/// </summary>
public class Cart : AggregateRoot
{
    // Internal collection used to manage cart items while preventing direct modification from outside.
    private readonly List<CartItem> _items = new();

    /// <summary>
    /// Gets the identifier of the customer who owns this cart.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Provides read-only access to the items inside the cart.
    /// </summary>
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    // Private constructor required by ORM frameworks for object materialization.
    private Cart()
    {
        
    }

    /// <summary>
    /// Creates a new cart instance for the specified customer.
    /// </summary>
    /// <param name="customerId">The identifier of the customer owning the cart.</param>
    public Cart(Guid customerId)
    {
        CustomerId = customerId;
    }

    /// <summary>
    /// Creates and validates a new Cart aggregate.
    /// </summary>
    /// <param name="customerId">The identifier of the customer.</param>
    /// <returns>A new Cart instance.</returns>
    public static Cart Create(Guid customerId)
    {
        // Ensures that a valid customer is assigned to the cart.
        if (customerId == Guid.Empty)
            throw new Exception("CustomerId cannot be empty.");

        return new Cart(customerId);
    }


    /// <summary>
    /// Adds a product item to the cart.
    /// If the product already exists, its quantity will be increased.
    /// Otherwise, a new cart item will be created.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="quantity">The quantity to add.</param>
    /// <param name="price">The unit price of the product.</param>
    public void AddItem(
        Guid productId,
        ProductName productName,
        Quantity quantity,
        decimal price)
    {
        // Checks whether the product already exists in the cart.
        var existingItem = _items
            .FirstOrDefault(x => x.ProductId == productId);


        if (existingItem != null)
        {
            // Increases the quantity of the existing cart item.
            existingItem.IncreaseQuantity(quantity.Value);
        }
        else
        {
            // Creates a new cart item and adds it to the collection.
            var item = new CartItem(
                productId,
                productName,
                quantity,
                price
            );

            _items.Add(item);
        }


        // Publishes a domain event after successfully adding an item.
        AddDomainEvent(
            new ItemAddedToCartEvent(
                Id,
                productId,
                quantity
            )
        );
    }


    /// <summary>
    /// Removes a product from the cart.
    /// </summary>
    /// <param name="productId">The identifier of the product to remove.</param>
    public void RemoveItem(Guid productId)
    {
        // Finds the requested product in the cart.
        var item = _items
            .FirstOrDefault(x => x.ProductId == productId);


        // Prevents removing a product that does not exist.
        if (item == null)
            throw new Exception("Product does not exist in cart.");


        _items.Remove(item);


        // Publishes a domain event after removing an item.
        AddDomainEvent(
            new ItemRemovedFromCartEvent(
                Id,
                productId,
                item.Quantity
            )
        );
    }


    /// <summary>
    /// Calculates the total price of all items in the cart.
    /// </summary>
    /// <returns>The total cart price.</returns>
    public decimal GetTotalPrice()
    {
        return _items.Sum(x => x.GetTotalPrice());
    }


    /// <summary>
    /// Removes all items from the cart.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }
}