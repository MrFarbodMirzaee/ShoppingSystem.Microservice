using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ShoppingSystem.Microservice.Notification.Sms.Repository;

/// <summary>
/// ToDo: adjust this for microservice project
/// </summary>
public class SmsRepository : ISmsService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber;

    public SmsRepository(
        string accountSid,
        string authToken,
        string fromNumber)
    {
        _accountSid = accountSid;
        _authToken = authToken;
        _fromNumber = fromNumber;
    }

    public async Task SendAsync(SmsRequestDto request,CancellationToken ct)
    {
        TwilioClient.Init(_accountSid, _authToken);

        await MessageResource.CreateAsync(
            body: request.Message,
            from: new PhoneNumber(_fromNumber),
            to: new PhoneNumber(request.PhoneNumber)
        );
    }
}