using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Hotel;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HotelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region POST - Create Hotel

        /// <summary>
        /// Create a new hotel
        /// </summary>
        /// <param name="command">Hotel creation data</param>
        /// <returns>Created hotel data</returns>
        /// <response code="200">Hotel created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/hotels
        ///     {
        ///       "name": "Grand Nile Hotel",
        ///       "description": "5-star luxury hotel with Nile view",
        ///       "city": "Cairo",
        ///       "country": "Egypt",
        ///       "street": "26th July Street",
        ///       "postalCode": "11511",
        ///       "latitude": 30.0444,
        ///       "longitude": 31.2357,
        ///       "email": "info@grandnile.com",
        ///       "phone": "+20-2-1234-5678",
        ///       "website": "https://grandnile.com",
        ///       "mainImageUrl": "https://cdn.example.com/hotels/grand-nile/main.jpg",
        ///       "mainImageAlt": "Grand Nile Hotel exterior",
        ///       "galleryUrls": [
        ///         "https://cdn.example.com/hotels/grand-nile/gallery1.jpg",
        ///         "https://cdn.example.com/hotels/grand-nile/gallery2.jpg"
        ///       ],
        ///       "starRating": 5,
        ///       "categoryId": "guid-of-hotel-category"
        ///     }
        ///     
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CreateHotelResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion

        #region GET - Search Hotels

        /// <summary>
        /// Search hotels with various filters
        /// Supports city/country filter, star rating filter, and nearby search
        /// </summary>
        /// <param name="query">Search parameters</param>
        /// <returns>List of hotels matching criteria</returns>
        /// <response code="200">Search completed successfully</response>
        /// <remarks>
        /// Sample requests:
        /// 
        /// 1. Search by city:
        ///     GET /api/hotels/search?city=Cairo&amp;page=1&amp;pageSize=20
        ///     
        /// 2. Search by city and star rating:
        ///     GET /api/hotels/search?city=Cairo&amp;minStarRating=4&amp;page=1&amp;pageSize=20
        ///     
        /// 3. Find nearby hotels (within 5km of coordinates):
        ///     GET /api/hotels/search?latitude=30.0444&amp;longitude=31.2357&amp;radiusKm=5&amp;page=1&amp;pageSize=20
        ///     
        /// 4. Combined search (nearby + filters):
        ///     GET /api/hotels/search?city=Cairo&amp;minStarRating=4&amp;latitude=30.0444&amp;longitude=31.2357&amp;radiusKm=10&amp;page=1&amp;pageSize=20
        ///     
        /// </remarks>
        //[HttpGet("search")]
        //[ProducesResponseType(typeof(SearchHotelsResponse), 200)]
        //public async Task<IActionResult> SearchHotels([FromQuery] SearchHotelsQuery query)
        //{
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        #endregion

        #region GET - Get Hotel by ID

        /// <summary>
        /// Get hotel details by ID
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>Hotel details with rooms</returns>
        /// <response code="200">Hotel found</response>
        /// <response code="404">Hotel not found</response>
        /// <remarks>
        /// Sample request:
        ///     GET /api/hotels/550e8400-e29b-41d4-a716-446655440000
        /// </remarks>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(HotelDetailsDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHotelById(Guid id)
        {
            // TODO: Implement GetHotelByIdQuery
            return Ok(new { message = "TODO: Get Hotel By ID", id });
        }

        #endregion

        #region GET - Get Nearby Hotels

        /// <summary>
        /// Get hotels near a specific location
        /// Shortcut for nearby search
        /// </summary>
        /// <param name="latitude">Latitude coordinate</param>
        /// <param name="longitude">Longitude coordinate</param>
        /// <param name="radiusKm">Search radius in kilometers (default: 5km)</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 20)</param>
        /// <returns>List of nearby hotels sorted by distance</returns>
        /// <response code="200">Search completed successfully</response>
        /// <remarks>
        /// Sample request:
        ///     GET /api/hotels/nearby?latitude=30.0444&amp;longitude=31.2357&amp;radiusKm=10
        ///     
        /// This will return hotels within 10km of the coordinates, sorted by distance (closest first)
        /// </remarks>
        //[HttpGet("nearby")]
        //[ProducesResponseType(typeof(SearchHotelsResponse), 200)]
        //public async Task<IActionResult> GetNearbyHotels(
        //    [FromQuery] double latitude,
        //    [FromQuery] double longitude,
        //    [FromQuery] double radiusKm = 5,
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 20)
        //{
        //    var query = new SearchHotelsQuery
        //    {
        //        Latitude = latitude,
        //        Longitude = longitude,
        //        RadiusKm = radiusKm,
        //        Page = page,
        //        PageSize = pageSize
        //    };

        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        #endregion
    }

    /// <summary>
    /// DTO for hotel details (placeholder - to be implemented)
    /// </summary>
    public class HotelDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string MainImageUrl { get; set; } = string.Empty;
        public List<string> GalleryUrls { get; set; } = new();
        public decimal? StarRating { get; set; }
        public List<RoomSummaryDto> Rooms { get; set; } = new();
    }

    /// <summary>
    /// DTO for room summary in hotel details
    /// </summary>
    public class RoomSummaryDto
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }

































}
