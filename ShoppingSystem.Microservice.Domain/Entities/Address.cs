using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.Entities;

/// <summary>
/// Represents a customer address entity.
/// Stores address information required for shipping or customer-related operations.
/// </summary>
public class Address : BaseEntity
{

    // Private constructor required by ORM frameworks for entity materialization.
    private Address()
    {
        
    }


    /// <summary>
    /// Gets the street address.
    /// </summary>
    public string Street { get; private set; }


    /// <summary>
    /// Gets the city of the address.
    /// </summary>
    public string City { get; private set; }


    /// <summary>
    /// Gets the state or region of the address.
    /// </summary>
    public string State { get; private set; }


    /// <summary>
    /// Gets the country of the address.
    /// </summary>
    public string Country { get; private set; }


    /// <summary>
    /// Gets the postal code of the address.
    /// </summary>
    public string PostalCode { get; private set; }


    /// <summary>
    /// Creates a new address entity.
    /// </summary>
    /// <param name="street">The street address.</param>
    /// <param name="city">The city name.</param>
    /// <param name="state">The state or region name.</param>
    /// <param name="country">The country name.</param>
    /// <param name="postalCode">The postal code.</param>
    public Address(
        string street,
        string city,
        string state,
        string country,
        string postalCode)
    {
        // Ensures that required address fields contain valid values.
        if (string.IsNullOrWhiteSpace(street))
            throw new Exception("Street cannot be empty.");

        if (string.IsNullOrWhiteSpace(city))
            throw new Exception("City cannot be empty.");

        if (string.IsNullOrWhiteSpace(country))
            throw new Exception("Country cannot be empty.");

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new Exception("Postal code cannot be empty.");


        // Stores normalized address information.
        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
        PostalCode = postalCode.Trim();
    }


    /// <summary>
    /// Updates the address information.
    /// </summary>
    /// <param name="street">The updated street address.</param>
    /// <param name="city">The updated city name.</param>
    /// <param name="state">The updated state or region name.</param>
    /// <param name="country">The updated country name.</param>
    /// <param name="postalCode">The updated postal code.</param>
    public void Update(
        string street,
        string city,
        string state,
        string country,
        string postalCode)
    {
        // Updates address fields after trimming input values.
        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
        PostalCode = postalCode.Trim();
    }
}