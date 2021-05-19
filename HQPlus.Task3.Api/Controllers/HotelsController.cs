using System;
using System.Net;
using System.Threading.Tasks;
using HQPlus.Data.Model;
using HQPlus.Task3.Api.Infrastructure;
using HQPlus.Task3.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HQPlus.Task3.Api.Controllers {
    
    [Route("/api/[controller]")]
    [ApiController]
    public sealed class HotelsController : ControllerBase {

        private readonly IHotelWithRatesRepository _hotelsRepo;

        public HotelsController(IHotelWithRatesRepository hotelsRepo) {
            _hotelsRepo = hotelsRepo;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(HotelWithRates), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<HotelWithRates>> GetHotel(long id, [ModelBinder(typeof(ISODateBinder))] DateTime arrivalDate) {
            var hotelWithRates = await _hotelsRepo.GetAsync(id, arrivalDate);
            if (hotelWithRates == null) {
                return NotFound();
            }

            return hotelWithRates;
        }

    }
}