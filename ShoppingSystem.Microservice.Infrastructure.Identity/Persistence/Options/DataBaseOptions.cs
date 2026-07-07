#nullable disable
namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Options;

public class DataBaseOptions
{
    public string Provider { get; set; }
    public string ConnectionStringName { get; set; }
    public string IdentityConnectionStringName { get; set; }
}