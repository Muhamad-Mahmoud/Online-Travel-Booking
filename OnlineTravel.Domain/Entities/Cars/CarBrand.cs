using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Domain.Entities.Cars;

public class CarBrand : BaseEntity
{
	public string Name { get; set; } = string.Empty;

	public string? Logo { get; set; }

	public bool IsActive { get; set; } = true;

	// Navigation Properties

	public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}




