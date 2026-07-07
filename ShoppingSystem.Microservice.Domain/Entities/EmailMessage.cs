namespace ShoppingSystem.Microservice.Domain.Entities;

/// <summary>
/// Represents an email message entity.
/// Contains the information required to send an email notification.
/// </summary>
public class EmailMessage
{
    /// <summary>
    /// Gets or sets the recipient email address.
    /// </summary>
    public string To { get; set; } 


    /// <summary>
    /// Gets or sets the email subject.
    /// </summary>
    public string Subject { get; set; } 


    /// <summary>
    /// Gets or sets the email body content.
    /// </summary>
    public string Body { get; set; } 


    /// <summary>
    /// Gets or sets whether the email body should be interpreted as HTML content.
    /// </summary>
    public bool IsHtml { get; set; } = true;
}