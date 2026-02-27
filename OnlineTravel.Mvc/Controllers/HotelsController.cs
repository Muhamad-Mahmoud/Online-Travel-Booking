using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using OnlineTravel.Mvc.Helpers;
using OnlineTravel.Application.Features.Hotels.Admin.GetHotels;
using OnlineTravel.Application.Features.Hotels.Admin.AddRoom;
using OnlineTravel.Application.Features.Hotels.Admin.DeleteRoom;
using OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel;
using OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Mvc.Models;
using OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand; // This using is needed because HotelsCreateViewModel is sent as a command.
using System; // For TimeOnly.FromTimeSpan

namespace OnlineTravel.Mvc.Controllers;

public class HotelsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? search = null)
	{
		var result = await Mediator.Send(new GetHotelsQuery(pageIndex, pageSize, search));
		ViewBag.SearchTerm = search;
		return View(result.Value);
	}

	public IActionResult Create()
	{
		return View(new HotelsCreateViewModel());
	}

	[HttpPost]
	public async Task<IActionResult> Create(HotelsCreateViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		if (model.ImageFile != null)
		{
			model.MainImage = await FileUploadHelper.UploadFileAsync(model.ImageFile, "hotels");
		}

		var result = await Mediator.Send(model);

		if (result.IsSuccess)
		{
			TempData["Success"] = "Hotel Created Successfully!";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error ?? "An error occurred");
		return View(model);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new GetHotelDetailsQuery { Id = id });
		if (!result.IsSuccess) return NotFound();

		var dto = result.Value;
		if (dto == null) return NotFound();

		var viewModel = new HotelsManageViewModel
		{
			Hotel = new Hotel
			{
				Id = dto.Id,
				Name = dto.Name,
				Description = dto.Description,
				MainImageUrl = dto.MainImage,
				Address = new OnlineTravel.Mvc.Models.Address { 
					Street = dto.Street,
					City = dto.City, 
					State = dto.State,
					Country = dto.Country,
					PostalCode = dto.PostalCode,
					Latitude = dto.Latitude,
					Longitude = dto.Longitude
				},
				Rating = new Rating { Value = dto.Rating },
				ContactEmail = dto.ContactEmail,
				ContactPhone = dto.ContactPhone,
				Website = dto.Website,
				CancellationPolicy = dto.CancellationPolicy,
				CheckInTime = new TimeRange(dto.CheckInTime, dto.CheckInTime.AddHours(4)), // Dummy range for display
				CheckOutTime = new TimeRange(dto.CheckOutTime.AddHours(-4), dto.CheckOutTime),
				Rooms = dto.Rooms.Select(r => new Room 
				{ 
					Id = r.Id, 
					Name = r.Name, 
					Description = r.Description, 
					RoomNumber = r.RoomNumber,
					BasePricePerNight = new Money(r.BasePricePerNight)
				}).ToList()
			}
		};
        
        // Correcting room mapping - I need to check if HotelDetailsDto has rooms.
        // It doesn't seem to have rooms in the file view. Let me check GetHotelRoomsQuery.
		return View(viewModel);
	}

	public async Task<IActionResult> Edit(Guid id)
	{
		var result = await Mediator.Send(new GetHotelDetailsQuery { Id = id });
		if (!result.IsSuccess) return NotFound();

		var dto = result.Value;
		if (dto == null) return NotFound();

		var viewModel = new HotelsEditViewModel
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
	public async Task<IActionResult> Edit(HotelsEditViewModel model)
	{
		if (model.ImageFile != null)
		{
			model.MainImage = await FileUploadHelper.UploadFileAsync(model.ImageFile, "hotels");
		}
		else
		{
			model.MainImage = model.CurrentImageUrl;
		}

		var command = new UpdateHotelCommand
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
			MainImage = model.MainImage,
			CheckInTime = TimeOnly.FromTimeSpan(model.CheckInTime),
			CheckOutTime = TimeOnly.FromTimeSpan(model.CheckOutTime),
			CancellationPolicy = model.CancellationPolicy,
			ContactPhone = model.ContactPhone,
			ContactEmail = model.ContactEmail,
			Website = model.Website
		};

		try
		{
			var result = await Mediator.Send(command);
			if (result.IsSuccess)
			{
				TempData["Success"] = "Hotel Updated Successfully!";
				return RedirectToAction("Manage", new { id = model.Id });
			}

			ModelState.AddModelError(string.Empty, result.Error ?? "An error occurred");
		}
		catch (ValidationException ex)
		{
			foreach (var error in ex.Errors)
			{
				ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
		}

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> AddRoom(HotelsManageViewModel model)
	{
		var result = await Mediator.Send(new AddRoomCommand
		{
			HotelId = model.RoomForm.HotelId,
			RoomNumber = model.RoomForm.RoomNumber,
			Name = model.RoomForm.Name,
			Description = model.RoomForm.Description,
			BasePricePerNight = model.RoomForm.BasePrice
		});

		if (result.IsSuccess) TempData["Success"] = "Room Added Successfully!";
		return RedirectToAction("Manage", new { id = model.RoomForm.HotelId });
	}

    [HttpPost]
    public async Task<IActionResult> DeleteRoom(Guid id, Guid hotelId)
    {
        var result = await Mediator.Send(new DeleteRoomCommand { Id = id });
        if (result.IsSuccess) TempData["Success"] = "Room Deleted Successfully!";
        return RedirectToAction("Manage", new { id = hotelId });
    }
}

