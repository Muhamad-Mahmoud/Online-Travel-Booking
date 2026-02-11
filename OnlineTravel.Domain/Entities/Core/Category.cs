using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Core;

public class Category : BaseEntity
{
    public CategoryType Type { get; set; } 

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ImageUrl? Image { get; set; }


    public bool IsActive { get; set; } = true;
}




