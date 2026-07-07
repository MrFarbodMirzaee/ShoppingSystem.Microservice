namespace ShoppingSystem.Microservice.Application.Events;

public sealed record OrderCreatedIntegrationEvent
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public decimal TotalAmount { get; init; }

    public List<OrderItemDto> Items { get; init; } = new();

    public sealed record OrderItemDto
    {
        public Guid ProductId { get; init; }
        public byte Quantity { get; init; }
    }
}