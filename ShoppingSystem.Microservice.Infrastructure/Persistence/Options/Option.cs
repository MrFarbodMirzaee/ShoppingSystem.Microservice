using Infrastructure.Persistence.Enums;

namespace Infrastructure.Persistence.Options;

public class Option
{
    public Option() 
    {
    }

    public Provider Provider { get; set; }

    public string ConnectionString { get; set; }
    public string ConnectionStringName { get; set; }
}