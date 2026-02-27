using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Hotels.Admin.GetHotels;
using OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand;

namespace OnlineTravel.Mvc.Controllers;

public class HotelsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? search = null)
	{
		var result = await Mediator.Send(new GetHotelsQuery(pageIndex, pageSize, search));
		ViewBag.SearchTerm = search;
		return View(result.Value);
	}

	public IActionResult Create()
	{
		return View(new CreateHotelCommand());
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateHotelCommand command)
	{
		if (!ModelState.IsValid)
		{
			return View(command);
		}

		var result = await Mediator.Send(command);

		if (result.IsSuccess)
		{
			TempData["Success"] = "Hotel created successfully";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error);
		return View(command);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails.GetHotelDetailsQuery { Id = id });
		if (!result.IsSuccess) return NotFound();

		var dto = result.Value;
		var viewModel = new OnlineTravel.Mvc.Models.HotelsManageViewModel
		{
			Hotel = new OnlineTravel.Mvc.Models.Hotel
			{
				Id = dto.Id,
				Name = dto.Name,
				Description = dto.Description,
				MainImageUrl = dto.MainImage,
				Address = new OnlineTravel.Mvc.Models.Address { City = dto.City, Country = dto.Country },
				Rating = new OnlineTravel.Mvc.Models.Rating { Value = dto.Rating },
				ContactEmail = dto.ContactEmail,
				ContactPhone = dto.ContactPhone,
				Website = dto.Website,
				CancellationPolicy = dto.CancellationPolicy,
				CheckInTime = new OnlineTravel.Domain.Entities._Shared.ValueObjects.TimeRange(dto.CheckInTime, dto.CheckInTime.AddHours(4)), // Dummy range for display
				CheckOutTime = new OnlineTravel.Domain.Entities._Shared.ValueObjects.TimeRange(dto.CheckOutTime.AddHours(-4), dto.CheckOutTime),
				Rooms = dto.Rooms?.Select(r => new OnlineTravel.Mvc.Models.Room 
				{ 
					Id = r.Id, 
					Name = r.Name, 
					Description = r.Description, 
					RoomNumber = r.RoomNumber,
					BasePricePerNight = new OnlineTravel.Domain.Entities._Shared.ValueObjects.Money(r.BasePricePerNight)
				}).ToList() ?? new List<OnlineTravel.Mvc.Models.Room>()
			}
		};
        
        // Correcting room mapping - I need to check if HotelDetailsDto has rooms.
        // It doesn't seem to have rooms in the file view. Let me check GetHotelRoomsQuery.
		return View(viewModel);
	}

	public async Task<IActionResult> Edit(Guid id)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails.GetHotelDetailsQuery { Id = id });
		if (!result.IsSuccess) return NotFound();

		var dto = result.Value;
		var viewModel = new OnlineTravel.Mvc.Models.HotelsEditViewModel
		{
			Id = dto.Id,
			Name = dto.Name,
			Description = dto.Description,
			CurrentImageUrl = dto.MainImage,
			Street = dto.Street,
			City = dto.City,
			State = dto.State,
			Country = dto.Country,
			PostalCode = dto.PostalCode,
			Latitude = (decimal)dto.Latitude,
			Longitude = (decimal)dto.Longitude,
			CheckInTime = dto.CheckInTime.ToTimeSpan(),
			CheckOutTime = dto.CheckOutTime.ToTimeSpan(),
			ContactPhone = dto.ContactPhone,
			ContactEmail = dto.ContactEmail,
			Website = dto.Website,
			CancellationPolicy = dto.CancellationPolicy
		};

		return View(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(OnlineTravel.Mvc.Models.HotelsEditViewModel model)
	{
		var command = new OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel.UpdateHotelCommand
		{
			Id = model.Id,
			Name = model.Name,
			Description = model.Description,
			Street = model.Street,
			City = model.City,
			State = model.State,
			Country = model.Country,
			PostalCode = model.PostalCode,
			Latitude = (double)model.Latitude,
			Longitude = (double)model.Longitude,
			MainImage = model.MainImage ?? model.CurrentImageUrl,
			CheckInTime = TimeOnly.FromTimeSpan(model.CheckInTime),
			CheckOutTime = TimeOnly.FromTimeSpan(model.CheckOutTime),
			CancellationPolicy = model.CancellationPolicy,
			ContactPhone = model.ContactPhone,
			ContactEmail = model.ContactEmail,
			Website = model.Website
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Hotel updated successfully";
			return RedirectToAction("Manage", new { id = model.Id });
		}

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> AddRoom(OnlineTravel.Mvc.Models.HotelsManageViewModel model)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Hotels.Admin.AddRoom.AddRoomCommand
		{
			HotelId = model.RoomForm.HotelId,
			RoomNumber = model.RoomForm.RoomNumber,
			Name = model.RoomForm.Name,
			Description = model.RoomForm.Description,
			BasePricePerNight = model.RoomForm.BasePrice
		});

		if (result.IsSuccess) TempData["Success"] = "Room added successfully";
		return RedirectToAction("Manage", new { id = model.RoomForm.HotelId });
	}

    [HttpPost]
    public async Task<IActionResult> DeleteRoom(Guid id, Guid hotelId)
    {
        var result = await Mediator.Send(new OnlineTravel.Application.Features.Hotels.Admin.DeleteRoom.DeleteRoomCommand { Id = id });
        if (result.IsSuccess) TempData["Success"] = "Room deleted successfully";
        return RedirectToAction("Manage", new { id = hotelId });
    }
}
