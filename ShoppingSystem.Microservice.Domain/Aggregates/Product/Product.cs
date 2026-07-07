using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Aggregates.Product;

/// <summary>
/// Represents the Product aggregate root.
/// Responsible for managing product information, pricing, availability,
/// images, and related inventory data.
/// </summary>
public class Product : AggregateRoot
{
    // Private constructor required by ORM frameworks for entity materialization.
    private Product()
    {
    }


    /// <summary>
    /// Creates a new product instance.
    /// </summary>
    /// <param name="name">The product name value object.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="categoryId">The identifier of the product category.</param>
    /// <returns>A new Product instance.</returns>
    public static Product Create(
        ProductName name,
        string description,
        Money price,
        Guid categoryId)
    {
        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            CategoryId = categoryId,
            IsAvailable = true
        };
    }
    

    /// <summary>
    /// Updates the product information.
    /// </summary>
    /// <param name="name">The updated product name.</param>
    /// <param name="description">The updated product description.</param>
    /// <param name="price">The updated product price.</param>
    /// <param name="categoryId">The updated category identifier.</param>
    /// <param name="isAvailable">The updated availability status.</param>
    public void Update(
        ProductName name,
        string description,
        Money price,
        Guid categoryId,
        bool isAvailable)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        IsAvailable = isAvailable;
    }


    /// <summary>
    /// Gets the product name.
    /// </summary>
    public ProductName Name { get; private set; }


    /// <summary>
    /// Gets the product description.
    /// </summary>
    public string Description { get; private set; }


    /// <summary>
    /// Gets the product price.
    /// </summary>
    public Money Price { get; private set; }


    /// <summary>
    /// Gets the identifier of the category this product belongs to.
    /// </summary>
    public Guid CategoryId { get; private set; }


    /// <summary>
    /// Gets whether the product is available for purchase.
    /// </summary>
    public bool IsAvailable { get; private set; }


    /// <summary>
    /// Navigation property representing the product inventory.
    /// </summary>
    public Inventory.Inventory Inventory { get; set; }
    

    // Internal collection used to manage product images.
    private readonly List<ProductImage> _images = new();


    /// <summary>
    /// Provides read-only access to product images.
    /// </summary>
    public IReadOnlyCollection<ProductImage> Images =>
        _images.AsReadOnly();
    

    // Internal collection used to track cart items containing this product.
    private readonly List<CartItem> _cartItems;


    /// <summary>
    /// Provides read-only access to cart items associated with this product.
    /// </summary>
    public IReadOnlyCollection<CartItem> CartItems => _cartItems;
}