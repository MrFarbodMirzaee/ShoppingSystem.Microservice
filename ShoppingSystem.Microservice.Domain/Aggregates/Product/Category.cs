using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.Aggregates.Product;

/// <summary>
/// Represents the Category aggregate root.
/// Responsible for managing product categorization, category information,
/// and category activation status.
/// </summary>
public class Category : AggregateRoot
{
    // Internal collection used to store product identifiers assigned to this category.
    private readonly List<Guid> _productIds = new();


    /// <summary>
    /// Provides read-only access to product identifiers associated with this category.
    /// </summary>
    public IReadOnlyCollection<Guid> ProductIds => _productIds;


    /// <summary>
    /// Gets the category name.
    /// </summary>
    public string Name { get; private set; }


    /// <summary>
    /// Gets the category description.
    /// </summary>
    public string Description { get; private set; }


    /// <summary>
    /// Gets the identifier of the parent category if this category has a parent.
    /// </summary>
    public Guid? ParentCategoryId { get; private set; }


    /// <summary>
    /// Gets whether the category is active.
    /// </summary>
    public bool IsActive { get; private set; }


    // Private constructor required by ORM frameworks for entity materialization.
    private Category()
    {
        
    }


    /// <summary>
    /// Creates a new category instance.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <param name="description">The category description.</param>
    /// <param name="parentCategoryId">The optional parent category identifier.</param>
    public Category(
        string name,
        string description,
        Guid? parentCategoryId = null)
    {
        // Ensures that a category always has a valid name.
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Category name cannot be empty.");

        // Stores normalized category information.
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        ParentCategoryId = parentCategoryId;
        IsActive = true;
    }


    /// <summary>
    /// Adds a product identifier to the category.
    /// </summary>
    /// <param name="productId">The identifier of the product to add.</param>
    public void AddProduct(Guid productId)
    {
        // Prevents adding duplicate products to the category.
        if (!_productIds.Contains(productId))
        {
            _productIds.Add(productId);
        }
    }


    /// <summary>
    /// Removes a product identifier from the category.
    /// </summary>
    /// <param name="productId">The identifier of the product to remove.</param>
    public void RemoveProduct(Guid productId)
    {
        _productIds.Remove(productId);
    }


    /// <summary>
    /// Updates category information.
    /// </summary>
    /// <param name="name">The new category name.</param>
    /// <param name="description">The new category description.</param>
    public void Update(
        string name,
        string description)
    {
        // Ensures that the updated category name is valid.
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Category name cannot be empty.");

        // Updates category information after normalization.
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }


    /// <summary>
    /// Disables the category.
    /// Disabled categories cannot be considered active.
    /// </summary>
    public void Disable()
    {
        IsActive = false;
    }


    /// <summary>
    /// Enables the category.
    /// </summary>
    public void Enable()
    {
        IsActive = true;
    }
}