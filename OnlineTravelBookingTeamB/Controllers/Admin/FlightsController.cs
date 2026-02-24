using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Infrastructure.Persistence.Context;
using MediatR;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravelBookingTeamB.Models;
using OnlineTravel.Application.Features.Flight.Flights.CreateFlight;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/Flights")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FlightsController : Controller
    {
        private readonly OnlineTravelDbContext _context;
        private readonly IMediator _mediator;

        public FlightsController(OnlineTravelDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(string? search = null, string? status = null)
        {
            var query = _context.Flights
                .Include(f => f.Carrier)
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Seats)
                .Include(f => f.Fares)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(f => f.FlightNumber.Value.ToLower().Contains(term) ||
                                         f.Carrier.Name.ToLower().Contains(term) ||
                                         f.OriginAirport.Name.ToLower().Contains(term) ||
                                         f.DestinationAirport.Name.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<FlightStatus>(status, out var flightStatus))
            {
                query = query.Where(f => f.Status == flightStatus);
            }

            var flights = await query.OrderByDescending(f => f.Schedule.Start).ToListAsync();
            ViewBag.SearchTerm = search;
            ViewBag.Status = status;
            ViewBag.FlightStatuses = Enum.GetNames(typeof(FlightStatus)).ToList();
            return View("~/Views/Admin/Flights/Flights/Index.cshtml", flights);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            await LoadFlightDropdowns();
            return View("~/Views/Admin/Flights/Flights/Create.cshtml", new CreateFlightViewModel());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateFlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadFlightDropdowns();
                return View("~/Views/Admin/Flights/Flights/Create.cshtml", model);
            }

            try
            {
                var baggageRules = string.IsNullOrWhiteSpace(model.BaggageRulesText)
                    ? new List<string>()
                    : model.BaggageRulesText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                var command = new CreateFlightCommand
                {
                    FlightNumber = model.FlightNumber,
                    CarrierId = model.CarrierId,
                    OriginAirportId = model.OriginAirportId,
                    DestinationAirportId = model.DestinationAirportId,
                    DepartureTime = model.DepartureTime,
                    ArrivalTime = model.ArrivalTime,
                    BaggageRules = baggageRules,
                    Refundable = model.Refundable,
                    CategoryId = model.CategoryId,
                    Gate = model.Gate,
                    Terminal = model.Terminal,
                    AircraftType = model.AircraftType
                };

                await _mediator.Send(command);
                TempData["Success"] = "Flight created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating flight: " + ex.Message);
                await LoadFlightDropdowns();
                return View("~/Views/Admin/Flights/Flights/Create.cshtml", model);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var flight = await _context.Flights
                .Include(f => f.Metadata)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null) return NotFound();

            await LoadFlightDropdowns();

            var model = new EditFlightViewModel
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber?.Value ?? "",
                CarrierId = flight.CarrierId,
                OriginAirportId = flight.OriginAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                DepartureTime = flight.Schedule?.Start ?? DateTime.Now,
                ArrivalTime = flight.Schedule?.End ?? DateTime.Now.AddHours(3),
                BaggageRulesText = string.Join(", ", flight.BaggageRules ?? new List<string>()),
                Refundable = flight.Refundable,
                CategoryId = flight.CategoryId,
                Status = flight.Status.ToString(),
                Gate = flight.Metadata?.Gate,
                Terminal = flight.Metadata?.Terminal,
                AircraftType = flight.Metadata?.AircraftType
            };

            ViewBag.Statuses = new SelectList(
                Enum.GetValues<FlightStatus>()
                    .Select(s => new { Value = s.ToString(), Text = s.ToString() }),
                "Value", "Text", model.Status);

            return View("~/Views/Admin/Flights/Flights/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(EditFlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadFlightDropdowns();
                ViewBag.Statuses = new SelectList(
                    Enum.GetValues<FlightStatus>()
                        .Select(s => new { Value = s.ToString(), Text = s.ToString() }),
                    "Value", "Text", model.Status);
                return View("~/Views/Admin/Flights/Flights/Edit.cshtml", model);
            }

            try
            {
                var flight = await _context.Flights
                    .Include(f => f.Metadata)
                    .FirstOrDefaultAsync(f => f.Id == model.Id);

                if (flight == null) return NotFound();

                flight.FlightNumber = new OnlineTravel.Domain.Entities.Flights.ValueObjects.FlightNumber(model.FlightNumber);
                flight.CarrierId = model.CarrierId;
                flight.OriginAirportId = model.OriginAirportId;
                flight.DestinationAirportId = model.DestinationAirportId;
                flight.Schedule = new OnlineTravel.Domain.Entities._Shared.ValueObjects.DateTimeRange(model.DepartureTime, model.ArrivalTime);
                flight.Refundable = model.Refundable;
                flight.CategoryId = model.CategoryId;

                if (Enum.TryParse<FlightStatus>(model.Status, out var status))
                    flight.Status = status;

                flight.BaggageRules = string.IsNullOrWhiteSpace(model.BaggageRulesText)
                    ? new List<string>()
                    : model.BaggageRulesText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                if (!string.IsNullOrWhiteSpace(model.Gate) || !string.IsNullOrWhiteSpace(model.Terminal) || !string.IsNullOrWhiteSpace(model.AircraftType))
                {
                    flight.Metadata = new OnlineTravel.Domain.Entities.Flights.ValueObjects.FlightMetadata(
                        model.Gate ?? "", model.Terminal ?? "", "", model.AircraftType ?? "");
                }

                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Flight updated successfully!";
                return RedirectToAction(nameof(Manage), new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating flight: " + ex.Message);
                await LoadFlightDropdowns();
                ViewBag.Statuses = new SelectList(
                    Enum.GetValues<FlightStatus>()
                        .Select(s => new { Value = s.ToString(), Text = s.ToString() }),
                    "Value", "Text", model.Status);
                return View("~/Views/Admin/Flights/Flights/Edit.cshtml", model);
            }
        }

        [HttpGet("/Admin/ManageFlight/{id}")]
        public IActionResult OldManageFlight(Guid id) => RedirectToAction(nameof(Manage), new { id });

        [HttpGet("Manage/{id}")]
        public async Task<IActionResult> Manage(Guid id)
        {
            var flight = await _context.Flights
                .Include(f => f.Carrier)
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Category)
                .Include(f => f.Seats)
                .Include(f => f.Fares)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null) return NotFound();

            var model = new Models.ManageFlightViewModel
            {
                Flight = flight,
                SeatForm = new Models.AddFlightSeatViewModel { FlightId = id },
                FareForm = new Models.AddFlightFareViewModel { FlightId = id }
            };

            return View("~/Views/Admin/Flights/Flights/Manage.cshtml", model);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null) return NotFound();

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Flight deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("AddSeat")]
        public async Task<IActionResult> AddSeat(Models.AddFlightSeatViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Manage), new { id = model.FlightId });
            }

            try
            {
                var cabinClass = Enum.TryParse<CabinClass>(model.CabinClass, out var parsed)
                    ? parsed
                    : CabinClass.Economy;

                var seatFeatures = string.IsNullOrWhiteSpace(model.SeatFeaturesText)
                    ? new List<string>()
                    : model.SeatFeaturesText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                var seat = new OnlineTravel.Domain.Entities.Flights.FlightSeat
                {
                    Id = Guid.NewGuid(),
                    FlightId = model.FlightId,
                    SeatLabel = model.SeatLabel,
                    CabinClass = cabinClass,
                    SeatFeatures = seatFeatures,
                    IsAvailable = true,
                    ExtraCharge = model.ExtraCharge > 0
                        ? new OnlineTravel.Domain.Entities._Shared.ValueObjects.Money(model.ExtraCharge)
                        : null
                };

                _context.Set<OnlineTravel.Domain.Entities.Flights.FlightSeat>().Add(seat);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Seat '{model.SeatLabel}' added successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error adding seat: " + ex.Message;
            }

            return RedirectToAction(nameof(Manage), new { id = model.FlightId });
        }

        [HttpPost("AddFare")]
        public async Task<IActionResult> AddFare(Models.AddFlightFareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Manage), new { id = model.FlightId });
            }

            try
            {
                var fare = new OnlineTravel.Domain.Entities.Flights.FlightFare
                {
                    Id = Guid.NewGuid(),
                    FlightId = model.FlightId,
                    BasePrice = new OnlineTravel.Domain.Entities._Shared.ValueObjects.Money(model.BasePrice, model.Currency),
                    SeatsAvailable = model.SeatsAvailable
                };

                _context.Set<OnlineTravel.Domain.Entities.Flights.FlightFare>().Add(fare);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Fare (${model.BasePrice} {model.Currency}) added successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error adding fare: " + ex.Message;
            }

            return RedirectToAction(nameof(Manage), new { id = model.FlightId });
        }

        [HttpPost("DeleteSeat")]
        public async Task<IActionResult> DeleteSeat(Guid id, Guid flightId)
        {
            var seat = await _context.Set<OnlineTravel.Domain.Entities.Flights.FlightSeat>().FindAsync(id);
            if (seat != null)
            {
                _context.Set<OnlineTravel.Domain.Entities.Flights.FlightSeat>().Remove(seat);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Seat deleted successfully.";
            }
            return RedirectToAction(nameof(Manage), new { id = flightId });
        }

        [HttpPost("DeleteFare")]
        public async Task<IActionResult> DeleteFare(Guid id, Guid flightId)
        {
            var fare = await _context.Set<OnlineTravel.Domain.Entities.Flights.FlightFare>().FindAsync(id);
            if (fare != null)
            {
                _context.Set<OnlineTravel.Domain.Entities.Flights.FlightFare>().Remove(fare);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fare deleted successfully.";
            }
            return RedirectToAction(nameof(Manage), new { id = flightId });
        }

        // --- Airports ---
        [HttpGet("Airports")]
        public async Task<IActionResult> Airports()
        {
            var airports = await _context.Airports.AsNoTracking().OrderBy(a => a.Name).ToListAsync();
            return View("~/Views/Admin/Flights/Airports/Index.cshtml", airports);
        }

        [HttpGet("Airports/Create")]
        public IActionResult CreateAirport()
        {
            return View("~/Views/Admin/Flights/Airports/Create.cshtml", new Models.CreateAirportViewModel());
        }

        [HttpPost("Airports/Create")]
        public async Task<IActionResult> CreateAirport(Models.CreateAirportViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Flights/Airports/Create.cshtml", model);

            try
            {
                var facilities = string.IsNullOrWhiteSpace(model.FacilitiesText)
                    ? new List<string>()
                    : model.FacilitiesText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                var command = new OnlineTravel.Application.Features.Flight.CreateAirport.CreateAirportCommand
                {
                    Code = model.Code,
                    Name = model.Name,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    Country = model.Country,
                    ZipCode = model.ZipCode,
                    Facilities = facilities
                };

                await _mediator.Send(command);
                TempData["Success"] = "Airport created successfully!";
                return RedirectToAction(nameof(Airports));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating airport: " + ex.Message);
                return View("~/Views/Admin/Flights/Airports/Create.cshtml", model);
            }
        }

        [HttpGet("Airports/Edit/{id}")]
        public async Task<IActionResult> EditAirport(Guid id)
        {
            var airport = await _context.Airports.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (airport == null) return NotFound();

            var model = new Models.EditAirportViewModel
            {
                Id = airport.Id,
                Code = airport.Code?.Value ?? "",
                Name = airport.Name,
                Street = airport.Address?.Street ?? "",
                City = airport.Address?.City ?? "",
                State = airport.Address?.State ?? "",
                Country = airport.Address?.Country ?? "",
                ZipCode = airport.Address?.PostalCode ?? "",
                FacilitiesText = string.Join(", ", airport.Facilities ?? new List<string>())
            };

            return View("~/Views/Admin/Flights/Airports/Edit.cshtml", model);
        }

        [HttpPost("Airports/Edit/{id}")]
        public async Task<IActionResult> EditAirport(Models.EditAirportViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Flights/Airports/Edit.cshtml", model);

            try
            {
                var facilities = string.IsNullOrWhiteSpace(model.FacilitiesText)
                    ? new List<string>()
                    : model.FacilitiesText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                var command = new OnlineTravel.Application.Features.Flight.Airport.UpdateAirport.UpdateAirportCommand
                {
                    Id = model.Id,
                    Code = model.Code,
                    Name = model.Name,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    Country = model.Country,
                    ZipCode = model.ZipCode,
                    Facilities = facilities
                };

                await _mediator.Send(command);
                TempData["Success"] = "Airport updated successfully!";
                return RedirectToAction(nameof(Airports));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating airport: " + ex.Message);
                return View("~/Views/Admin/Flights/Airports/Edit.cshtml", model);
            }
        }

        [HttpPost("Airports/Delete/{id}")]
        public async Task<IActionResult> DeleteAirport(Guid id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport != null)
            {
                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Airport deleted successfully.";
            }
            return RedirectToAction(nameof(Airports));
        }

        // --- Carriers ---
        [HttpGet("Carriers")]
        public async Task<IActionResult> Carriers()
        {
            var carriers = await _context.Carriers.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
            return View("~/Views/Admin/Flights/Carriers/Index.cshtml", carriers);
        }

        [HttpGet("Carriers/Create")]
        public IActionResult CreateCarrier()
        {
            return View("~/Views/Admin/Flights/Carriers/Create.cshtml", new Models.CreateCarrierViewModel());
        }

        [HttpPost("Carriers/Create")]
        public async Task<IActionResult> CreateCarrier(Models.CreateCarrierViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Flights/Carriers/Create.cshtml", model);

            try
            {
                var command = new OnlineTravel.Application.Features.Flight.Carrier.CreateCarrier.CreateCarrierCommand
                {
                    Name = model.Name,
                    Code = model.Code,
                    Logo = model.Logo,
                    Email = model.Email,
                    Phone = model.Phone
                };

                await _mediator.Send(command);
                TempData["Success"] = "Carrier created successfully!";
                return RedirectToAction(nameof(Carriers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating carrier: " + ex.Message);
                return View("~/Views/Admin/Flights/Carriers/Create.cshtml", model);
            }
        }

        [HttpPost("Carriers/Delete/{id}")]
        public async Task<IActionResult> DeleteCarrier(Guid id)
        {
            var carrier = await _context.Carriers.FindAsync(id);
            if (carrier != null)
            {
                _context.Carriers.Remove(carrier);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Carrier deleted successfully.";
            }
            return RedirectToAction(nameof(Carriers));
        }

        private async Task LoadFlightDropdowns()
        {
            var carriers = await _context.Carriers.AsNoTracking().ToListAsync();
            var airports = await _context.Airports.AsNoTracking().ToListAsync();
            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Flight)
                .AsNoTracking().ToListAsync();

            ViewBag.Carriers = new SelectList(carriers, "Id", "Name");
            ViewBag.Airports = new SelectList(airports, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
        }
    }
}
