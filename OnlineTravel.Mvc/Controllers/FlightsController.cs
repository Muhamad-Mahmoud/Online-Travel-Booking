using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Flights.GetFlights;
using OnlineTravel.Application.Features.Flight.Flights.CreateFlight;
using OnlineTravel.Application.Features.Flight.Airport.GetAllAirports;
using OnlineTravel.Application.Features.Flight.Airport.GetAirportById;
using OnlineTravel.Application.Features.Flight.Airport.UpdateAirport;
using OnlineTravel.Application.Features.Flight.CreateAirport;
using OnlineTravel.Application.Features.Flight.Carrier.GetAllCarriers;
using OnlineTravel.Application.Features.Flight.Carrier.CreateCarrier;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Mvc.Controllers;

public class FlightsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? search = null, string? status = null)
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
			TempData["Success"] = "Flight created successfully";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		await PopulateEditViewBags();
		return View("~/Views/Flights/Flights/Create.cshtml", command);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Flights.GetFlightById.GetFlightByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var flight = result.Value;
		var viewModel = new OnlineTravel.Mvc.Models.FlightManageViewModel
		{
			Flight = new OnlineTravel.Mvc.Models.FlightDto
			{
				Id = flight?.Id ?? Guid.Empty,
				FlightNumber = flight?.FlightNumber?.Value ?? "N/A",
				Carrier = flight?.Carrier != null ? new OnlineTravel.Mvc.Models.CarrierDto { Name = flight.Carrier.Name, Code = flight.Carrier.Code, Logo = flight.Carrier.Logo } : null,
				Schedule = new OnlineTravel.Mvc.Models.FlightScheduleDto { Start = flight.Schedule?.Start ?? DateTime.MinValue, End = flight.Schedule?.End ?? DateTime.MinValue },
				Status = flight.Status.ToString(),
				AircraftType = flight.Metadata?.AircraftType,
				Refundable = flight.Refundable,
				OriginAirport = flight.OriginAirport != null ? new OnlineTravel.Mvc.Models.AirportDto { Code = flight.OriginAirport.Code.Value, Name = flight.OriginAirport.Name } : null,
				DestinationAirport = flight.DestinationAirport != null ? new OnlineTravel.Mvc.Models.AirportDto { Code = flight.DestinationAirport.Code.Value, Name = flight.DestinationAirport.Name } : null,
				Seats = flight.Seats?.Select(s => new OnlineTravel.Mvc.Models.SeatDto { Id = s.Id, SeatLabel = s.SeatLabel, CabinClass = s.CabinClass.ToString(), ExtraCharge = s.ExtraCharge?.Amount ?? 0, IsAvailable = s.IsAvailable }).ToList() ?? [],
				Fares = flight.Fares?.Select(f => new OnlineTravel.Mvc.Models.FareDto { Id = f.Id, Amount = f.BasePrice?.Amount ?? 0, SeatsAvailable = f.SeatsAvailable }).ToList() ?? [],
				Metadata = flight.Metadata != null ? new OnlineTravel.Mvc.Models.FlightMetadataDto { Gate = flight.Metadata.Gate, Terminal = flight.Metadata.Terminal } : null,
				BaggageRules = string.Join(", ", flight.BaggageRules)
			}
		};

		return View("~/Views/Flights/Flights/Manage.cshtml", viewModel);
	}

	public async Task<IActionResult> Edit(Guid id)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Flights.GetFlightById.GetFlightByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var flight = result.Value;
		var viewModel = new OnlineTravel.Mvc.Models.FlightEditViewModel
		{
			Id = flight?.Id ?? Guid.Empty,
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
	public async Task<IActionResult> Edit(OnlineTravel.Mvc.Models.FlightEditViewModel model)
	{
		var command = new OnlineTravel.Application.Features.Flight.Flights.UpdateFlight.UpdateFlightCommand
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
			Status = Enum.Parse<OnlineTravel.Domain.Enums.FlightStatus>(model.Status),
			Gate = model.Gate,
			Terminal = model.Terminal,
			AircraftType = model.AircraftType
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Flight updated successfully";
			return RedirectToAction("Manage", new { id = model.Id });
		}

		await PopulateEditViewBags();
		return View("~/Views/Flights/Flights/Edit.cshtml", model);
	}

	[HttpPost]
	public async Task<IActionResult> AddSeat(OnlineTravel.Mvc.Models.FlightManageViewModel model)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Flights.Manage.AddSeatCommand
		{
			FlightId = model.SeatForm.FlightId,
			SeatLabel = model.SeatForm.SeatLabel,
			CabinClass = model.SeatForm.CabinClass,
			ExtraCharge = model.SeatForm.ExtraCharge
		});

		if (result.IsSuccess) TempData["Success"] = "Seat added successfully";
		return RedirectToAction("Manage", new { id = model.SeatForm.FlightId });
	}

	[HttpPost]
	public async Task<IActionResult> DeleteSeat(Guid id, Guid flightId)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Flights.Manage.DeleteSeatCommand { Id = id });
		if (result.IsSuccess) TempData["Success"] = "Seat deleted successfully";
		return RedirectToAction("Manage", new { id = flightId });
	}

	[HttpPost]
	public async Task<IActionResult> AddFare(OnlineTravel.Mvc.Models.FlightManageViewModel model)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Flights.Manage.AddFareCommand
		{
			FlightId = model.FareForm.FlightId,
			Amount = model.FareForm.Amount,
			SeatsAvailable = model.FareForm.SeatsAvailable
		});

		if (result.IsSuccess) TempData["Success"] = "Fare added successfully";
		return RedirectToAction("Manage", new { id = model.FareForm.FlightId });
	}

	[HttpPost]
	public async Task<IActionResult> DeleteFare(Guid id, Guid flightId)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Flights.Manage.DeleteFareCommand { Id = id });
		if (result.IsSuccess) TempData["Success"] = "Fare deleted successfully";
		return RedirectToAction("Manage", new { id = flightId });
	}

	public async Task<IActionResult> Airports(int pageIndex = 1, int pageSize = 10)
	{
		var result = await Mediator.Send(new GetAllAirportsQuery { PageIndex = pageIndex, PageSize = pageSize });
		
		var airports = result.Value.Select(dto => new Airport 
		{ 
			Id = dto.Id, 
			Name = dto.Name, 
			Code = OnlineTravel.Domain.Entities.Flights.ValueObjects.IataCode.Create(dto.Code),
			Address = new OnlineTravel.Domain.Entities._Shared.ValueObjects.Address { City = dto.City, Country = dto.Country }
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
			TempData["Success"] = "Airport created successfully";
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
			TempData["Success"] = "Airport updated successfully";
			return RedirectToAction(nameof(Airports));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		return View("~/Views/Flights/Airports/Edit.cshtml", command);
	}

	public async Task<IActionResult> Carriers(int pageIndex = 1, int pageSize = 10)
	{
		var result = await Mediator.Send(new GetAllCarriersQuery());
		var carriers = result.Value ?? new List<OnlineTravel.Domain.Entities.Flights.Carrier>();
		var paginatedResult = new PaginatedResult<Carrier>(pageIndex, pageSize, carriers.Count, carriers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());

		return View("~/Views/Flights/Carriers/Index.cshtml", paginatedResult);
	}

	public IActionResult CreateCarrier()
	{
		return View("~/Views/Flights/Carriers/Create.cshtml", new CreateCarrierCommand());
	}

	[HttpPost]
	public async Task<IActionResult> CreateCarrier(CreateCarrierCommand command)
	{
		if (!ModelState.IsValid) return View("~/Views/Flights/Carriers/Create.cshtml", command);

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Carrier created successfully";
			return RedirectToAction(nameof(Carriers));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		return View("~/Views/Flights/Carriers/Create.cshtml", command);
	}

	[HttpPost]
	public async Task<IActionResult> DeleteAirport(Guid id)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Airport.DeleteAirport.DeleteAirportCommand(id));
		if (result.IsSuccess) TempData["Success"] = "Airport deleted successfully";
		return RedirectToAction(nameof(Airports));
	}

	[HttpPost]
	public async Task<IActionResult> DeleteCarrier(Guid id)
	{
		var result = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Carrier.DeleteCarrier.DeleteCarrierCommand(id));
		if (result.IsSuccess) TempData["Success"] = "Carrier deleted successfully";
		return RedirectToAction(nameof(Carriers));
	}

	private async Task PopulateEditViewBags()
	{
		var carriers = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Carrier.GetAllCarriers.GetAllCarriersQuery());
		var airports = await Mediator.Send(new OnlineTravel.Application.Features.Flight.Airport.GetAllAirports.GetAllAirportsQuery());
		var categories = await Mediator.Send(new OnlineTravel.Application.Features.Categories.GetCategoriesByType.GetCategoriesByTypeQuery(OnlineTravel.Domain.Enums.CategoryType.Flight));

		ViewBag.Carriers = carriers.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(carriers.Value, "Id", "Name") : null;
		ViewBag.Airports = airports.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(airports.Value, "Id", "Name") : null;
		ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
		ViewBag.Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enum.GetValues<OnlineTravel.Domain.Enums.FlightStatus>());
	}
}
