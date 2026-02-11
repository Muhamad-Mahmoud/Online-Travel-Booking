using Microsoft.AspNetCore.Identity;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Users;

public class AppUser : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;

    public Address? Address { get; set; }


    public string? ProfilePicture { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }
}




