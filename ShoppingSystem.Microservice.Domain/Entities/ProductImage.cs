using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.Entities;

/// <summary>
/// Represents an image associated with a product.
/// Stores image metadata and binary content required for product image management.
/// </summary>
public class ProductImage : BaseEntity
{
    /// <summary>
    /// Gets the identifier of the related product.
    /// </summary>
    public Guid ProductId { get; private set; }


    /// <summary>
    /// Gets the original file name of the image.
    /// </summary>
    public string FileName { get; private set; }


    /// <summary>
    /// Gets the storage path of the image file.
    /// </summary>
    public string FilePath { get; private set; }


    /// <summary>
    /// Gets the MIME content type of the image.
    /// </summary>
    public string ContentType { get; private set; }
    

    /// <summary>
    /// Gets the binary data of the image.
    /// </summary>
    public byte[] Data { get; private set; }


    /// <summary>
    /// Gets the size of the image file in bytes.
    /// </summary>
    public long FileSize { get; private set; }


    /// <summary>
    /// Gets whether this image is the main image of the product.
    /// </summary>
    public bool IsMain { get; private set; }


    // Private constructor required by ORM frameworks for entity materialization.
    private ProductImage()
    {
        
    }


    /// <summary>
    /// Creates a new product image entity.
    /// </summary>
    /// <param name="productId">The identifier of the related product.</param>
    /// <param name="fileName">The image file name.</param>
    /// <param name="filePath">The image storage path.</param>
    /// <param name="contentType">The MIME content type of the file.</param>
    /// <param name="data">The binary image data.</param>
    /// <param name="fileSize">The size of the file in bytes.</param>
    /// <param name="isMain">Indicates whether the image is the primary product image.</param>
    public ProductImage(
        Guid productId,
        string fileName,
        string filePath,
        string contentType,
        byte[] data,
        long fileSize,
        bool isMain = false)
    {
        // Ensures that required image information is provided.
        if (string.IsNullOrWhiteSpace(fileName))
            throw new Exception("File name cannot be empty.");

        if (string.IsNullOrWhiteSpace(filePath))
            throw new Exception("File path cannot be empty.");

        if (string.IsNullOrWhiteSpace(contentType))
            throw new Exception("Content type cannot be empty.");

        if (fileSize <= 0)
            throw new Exception("File size must be greater than zero.");


        // Stores normalized image information.
        ProductId = productId;
        FileName = fileName.Trim();
        FilePath = filePath.Trim();
        ContentType = contentType.Trim();
        Data = data;
        FileSize = fileSize;
        IsMain = isMain;
    }


    /// <summary>
    /// Marks this image as the main product image.
    /// </summary>
    public void SetAsMain()
    {
        IsMain = true;
    }
}