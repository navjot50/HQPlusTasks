using System.Collections.Generic;
using System.Threading.Tasks;
using HQPlus.Data.Model;

namespace HQPlus.Task2.Repository {
    
    public interface IHotelRateRepository {

        IAsyncEnumerable<HotelRate> GetAllAsync();

    }
}