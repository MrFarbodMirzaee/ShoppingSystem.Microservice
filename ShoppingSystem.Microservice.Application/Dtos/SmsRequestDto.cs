namespace ShoppingSystem.Microservice.Application.Dtos;

public class SmsRequestDto
{
    public string PhoneNumber { get; set; }

    public string Message { get; set; }

    public string? TemplateName { get; set; }

    public Dictionary<string, string>? Parameters { get; set; }

    public string? ReferenceId { get; set; }
}