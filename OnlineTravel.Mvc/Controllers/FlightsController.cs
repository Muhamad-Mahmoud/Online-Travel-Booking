using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Domain.Exceptions;
using OnlineTravel.Mvc.Helpers;
using OnlineTravel.Application.Features.Categories.GetCategoriesByType;
using OnlineTravel.Application.Features.Flights.Airport.DeleteAirport;
using OnlineTravel.Application.Features.Flights.Airport.GetAllAirports;
using OnlineTravel.Application.Features.Flights.Airport.GetAirportById;
using OnlineTravel.Application.Features.Flights.Airport.UpdateAirport;
using OnlineTravel.Application.Features.Flights.CreateAirport;
using OnlineTravel.Application.Features.Flights.Carriers.DeleteCarrier;
using OnlineTravel.Application.Features.Flights.Carrier.GetAllCarriers;
using OnlineTravel.Application.Features.Flights.Carrier.CreateCarrier;
using OnlineTravel.Application.Features.Flights.Flights.GetFlightById;
using OnlineTravel.Application.Features.Flights.Flights.Manage;
using OnlineTravel.Application.Features.Flights.Flights.UpdateFlight;
using OnlineTravel.Application.Features.Flights.Flights.GetFlights;
using OnlineTravel.Application.Features.Flights.Flights.CreateFlight;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Mvc.Models;

namespace OnlineTravel.Mvc.Controllers;

public class FlightsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? search = null, string? status = null)
	{
		var result = await Mediator.Send(new GetFlightsQuery(pageIndex, pageSize, search, status));
		ViewBag.SearchTerm = search;
		ViewBag.Status = status;
		ViewBag.FlightStatuses = new List<string> { "Scheduled", "Cancelled", "Delayed", "InAir", "Landed" };
		return View("~/Views/Flights/Flights/Index.cshtml", result.Value);
	}

	public async Task<IActionResult> Create()
	{
		await PopulateEditViewBags();
		return View("~/Views/Flights/Flights/Create.cshtml", new CreateFlightCommand());
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateFlightCommand command)
	{
		if (!ModelState.IsValid)
		{
			await PopulateEditViewBags();
			return View("~/Views/Flights/Flights/Create.cshtml", command);
		}

		var result = await Mediator.Send(command);

		if (result.IsSuccess)
		{
			TempData["Success"] = "Flight Created Successfully!";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		await PopulateEditViewBags();
		return View("~/Views/Flights/Flights/Create.cshtml", command);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new GetFlightByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var flight = result.Value;
		if (flight == null) return NotFound();

		var viewModel = new FlightManageViewModel
		{
			Flight = new FlightDto
			{
				Id = flight.Id,
				FlightNumber = flight.FlightNumber?.Value ?? "N/A",
				Carrier = flight.Carrier != null ? new CarrierDto { Name = flight.Carrier.Name ?? "N/A", Code = flight.Carrier.Code ?? "N/A", Logo = flight.Carrier.Logo ?? string.Empty } : null,
				Schedule = new FlightScheduleDto { Start = flight.Schedule?.Start ?? DateTime.MinValue, End = flight.Schedule?.End ?? DateTime.MinValue },
				Status = flight.Status.ToString(),
				AircraftType = flight.Metadata?.AircraftType,
				Refundable = flight.Refundable,
				OriginAirport = flight.OriginAirport != null ? new AirportDto { Code = flight.OriginAirport.Code.Value, Name = flight.OriginAirport.Name } : null,
				DestinationAirport = flight.DestinationAirport != null ? new AirportDto { Code = flight.DestinationAirport.Code.Value, Name = flight.DestinationAirport.Name } : null,
				Seats = flight.Seats?.Select(s => new SeatDto { Id = s.Id, SeatLabel = s.SeatLabel, CabinClass = s.CabinClass.ToString(), ExtraCharge = s.ExtraCharge?.Amount ?? 0, IsAvailable = s.IsAvailable }).ToList() ?? [],
				Fares = flight.Fares?.Select(f => new FareDto { Id = f.Id, Amount = f.BasePrice?.Amount ?? 0, SeatsAvailable = f.SeatsAvailable }).ToList() ?? [],
				Metadata = flight.Metadata != null ? new FlightMetadataDto { Gate = flight.Metadata.Gate, Terminal = flight.Metadata.Terminal } : null,
				BaggageRules = string.Join(", ", flight.BaggageRules)
			}
		};

		return View("~/Views/Flights/Flights/Manage.cshtml", viewModel);
	}

	public async Task<IActionResult> Edit(Guid id)
	{
		var result = await Mediator.Send(new GetFlightByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var flight = result.Value;
		if (flight == null) return NotFound();

		var viewModel = new FlightEditViewModel
		{
			Id = flight.Id,
			FlightNumber = flight.FlightNumber?.Value ?? string.Empty,
			CarrierId = flight.CarrierId,
			OriginAirportId = flight.OriginAirportId,
			DestinationAirportId = flight.DestinationAirportId,
			DepartureTime = flight.Schedule?.Start ?? DateTime.MinValue,
			ArrivalTime = flight.Schedule?.End ?? DateTime.MinValue,
			BaggageRules = flight.BaggageRules,
			BaggageRulesText = string.Join(", ", flight.BaggageRules),
			Refundable = flight.Refundable,
			CategoryId = flight.CategoryId,
			Status = flight.Status.ToString(),
			Gate = flight.Metadata?.Gate,
			Terminal = flight.Metadata?.Terminal,
			AircraftType = flight.Metadata?.AircraftType
		};

		await PopulateEditViewBags();
		return View("~/Views/Flights/Flights/Edit.cshtml", viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(FlightEditViewModel model)
	{
		var command = new UpdateFlightCommand
		{
			Id = model.Id,
			FlightNumber = model.FlightNumber,
			CarrierId = model.CarrierId ?? Guid.Empty,
			OriginAirportId = model.OriginAirportId ?? Guid.Empty,
			DestinationAirportId = model.DestinationAirportId ?? Guid.Empty,
			DepartureTime = model.DepartureTime,
			ArrivalTime = model.ArrivalTime,
			BaggageRules = string.IsNullOrWhiteSpace(model.BaggageRulesText) ? [] : [.. model.BaggageRulesText.Split(',').Select(s => s.Trim())],
			Refundable = model.Refundable,
			CategoryId = model.CategoryId ?? Guid.Empty,
			Status = Enum.Parse<FlightStatus>(model.Status),
			Gate = model.Gate,
			Terminal = model.Terminal,
			AircraftType = model.AircraftType
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Flight Updated Successfully!";
			return RedirectToAction("Manage", new { id = model.Id });
		}

		await PopulateEditViewBags();
		return View("~/Views/Flights/Flights/Edit.cshtml", model);
	}

	[HttpPost]
	public async Task<IActionResult> AddSeat(FlightManageViewModel model)
	{
		var result = await Mediator.Send(new AddSeatCommand(
			model.SeatForm.FlightId,
			model.SeatForm.SeatLabel,
			model.SeatForm.CabinClass,
			model.SeatForm.ExtraCharge
		));

		if (result.IsSuccess) TempData["Success"] = "Seat Added Successfully!";
		return RedirectToAction("Manage", new { id = model.SeatForm.FlightId });
	}

	[HttpPost]
	public async Task<IActionResult> DeleteSeat(Guid id, Guid flightId)
	{
		var result = await Mediator.Send(new DeleteSeatCommand(id));
		if (result.IsSuccess) TempData["Success"] = "Seat Deleted Successfully!";
		return RedirectToAction("Manage", new { id = flightId });
	}

	[HttpPost]
	public async Task<IActionResult> AddFare(FlightManageViewModel model)
	{
		var result = await Mediator.Send(new AddFareCommand(
			model.FareForm.FlightId,
			"Standard", // FareName
			"Standard Fare", // Description
			model.FareForm.Amount,
			"USD", // Currency
			model.FareForm.SeatsAvailable
		));

		if (result.IsSuccess) TempData["Success"] = "Fare Added Successfully!";
		return RedirectToAction("Manage", new { id = model.FareForm.FlightId });
	}

	[HttpPost]
	public async Task<IActionResult> DeleteFare(Guid id, Guid flightId)
	{
		var result = await Mediator.Send(new DeleteFareCommand(id));
		if (result.IsSuccess) TempData["Success"] = "Fare Deleted Successfully!";
		return RedirectToAction("Manage", new { id = flightId });
	}

	public async Task<IActionResult> Airports(int pageIndex = 1, int pageSize = 5)
	{
		var result = await Mediator.Send(new GetAllAirportsQuery { PageIndex = pageIndex, PageSize = pageSize });
		
		var airports = result.Value.Select(dto => new Airport 
		{ 
			Id = dto.Id, 
			Name = dto.Name, 
			Code = OnlineTravel.Domain.Entities.Flights.ValueObjects.IataCode.Create(dto.Code),
			Address = new OnlineTravel.Domain.Entities._Shared.ValueObjects.Address(null, dto.City, null, dto.Country, null, null)
		}).ToList();

		var totalCount = airports.Count; 
		var paginatedResult = new PaginatedResult<Airport>(pageIndex, pageSize, totalCount, airports);

		return View("~/Views/Flights/Airports/Index.cshtml", paginatedResult);
	}

	public IActionResult CreateAirport()
	{
		return View("~/Views/Flights/Airports/Create.cshtml", new CreateAirportCommand());
	}

	[HttpPost]
	public async Task<IActionResult> CreateAirport(CreateAirportCommand command)
	{
		if (!ModelState.IsValid) return View("~/Views/Flights/Airports/Create.cshtml", command);

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Airport Created Successfully!";
			return RedirectToAction(nameof(Airports));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		return View("~/Views/Flights/Airports/Create.cshtml", command);
	}

	public async Task<IActionResult> EditAirport(Guid id)
	{
		var result = await Mediator.Send(new GetAirportByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var airport = result.Value;
		var command = new UpdateAirportCommand
		{
			Id = airport.Id,
			Name = airport.Name,
			Code = airport.Code,
			City = airport.City,
			Country = airport.Country
		};

		return View("~/Views/Flights/Airports/Edit.cshtml", command);
	}

	[HttpPost]
	public async Task<IActionResult> EditAirport(UpdateAirportCommand command)
	{
		if (!ModelState.IsValid) return View("~/Views/Flights/Airports/Edit.cshtml", command);

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Airport Updated Successfully!";
			return RedirectToAction(nameof(Airports));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		return View("~/Views/Flights/Airports/Edit.cshtml", command);
	}

	public async Task<IActionResult> Carriers(int pageIndex = 1, int pageSize = 5)
	{
		var result = await Mediator.Send(new GetAllCarriersQuery());
		var carriers = result.Value ?? [];
		var paginatedResult = new PaginatedResult<OnlineTravel.Domain.Entities.Flights.Carrier>(pageIndex, pageSize, carriers.Count, [.. carriers.Skip((pageIndex - 1) * pageSize).Take(pageSize)]);

		return View("~/Views/Flights/Carriers/Index.cshtml", paginatedResult);
	}

	public IActionResult CreateCarrier()
	{
		return View("~/Views/Flights/Carriers/Create.cshtml", new CarrierCreateViewModel());
	}

	[HttpPost]
	public async Task<IActionResult> CreateCarrier(CarrierCreateViewModel model)
	{
		if (!ModelState.IsValid) return View("~/Views/Flights/Carriers/Create.cshtml", model);

		if (model.LogoFile != null)
		{
			model.Logo = await FileUploadHelper.UploadFileAsync(model.LogoFile, "carriers");
		}

		var result = await Mediator.Send(model);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Carrier Created Successfully!";
			return RedirectToAction(nameof(Carriers));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		return View("~/Views/Flights/Carriers/Create.cshtml", model);
	}

	[HttpPost]
	public async Task<IActionResult> DeleteAirport(Guid id)
	{
		var result = await Mediator.Send(new DeleteAirportCommand(id));
		if (result.IsSuccess) TempData["Success"] = "Airport Deleted Successfully!";
		return RedirectToAction(nameof(Airports));
	}

	[HttpPost]
	public async Task<IActionResult> DeleteCarrier(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarrierCommand(id));
		if (result.IsSuccess) TempData["Success"] = "Carrier Deleted Successfully!";
		return RedirectToAction(nameof(Carriers));
	}

	private async Task PopulateEditViewBags()
	{
		var carriers = await Mediator.Send(new OnlineTravel.Application.Features.Flights.Carrier.GetAllCarriers.GetAllCarriersQuery());
		var airports = await Mediator.Send(new OnlineTravel.Application.Features.Flights.Airport.GetAllAirports.GetAllAirportsQuery());
		var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Flight));

		ViewBag.Carriers = carriers.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(carriers.Value, "Id", "Name") : null;
		ViewBag.Airports = airports.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(airports.Value, "Id", "Name") : null;
		ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
		ViewBag.Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enum.GetValues<FlightStatus>());
	}
}

