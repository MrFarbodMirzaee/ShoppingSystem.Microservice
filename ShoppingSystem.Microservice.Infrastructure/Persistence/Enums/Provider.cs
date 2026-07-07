namespace Infrastructure.Persistence.Enums;

public enum Provider : byte
{
    SqlServer = 0,
    MySql = 1,
    PostgresDb = 2,
    Oracle = 3,
    /// <summary>
    /// For xunit test In memory is not a bad idea
    /// </summary>
    InMemory = 4
}