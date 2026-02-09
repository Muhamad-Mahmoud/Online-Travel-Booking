namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record ContactInfo
{
    public EmailAddress? Email { get; init; }
    public PhoneNumber? Phone { get; init; }
    public Url? Website { get; init; }

    protected ContactInfo() { } // For EF

    public ContactInfo(EmailAddress? email = null, PhoneNumber? phone = null, Url? website = null)
    {
        Email = email;
        Phone = phone;
        Website = website;
    }
}





