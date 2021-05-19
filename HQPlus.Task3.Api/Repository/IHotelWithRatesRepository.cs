using System;
using System.Threading.Tasks;
using HQPlus.Data.Model;

namespace HQPlus.Task3.Api.Repository {
    public interface IHotelWithRatesRepository {
        Task<HotelWithRates> GetAsync(long id, DateTime arrivalDate);
    }
}