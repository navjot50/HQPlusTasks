using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HQPlus.Data.Extensions;
using HQPlus.Data.Model;
using HQPlus.Task2.Options;
using Microsoft.Extensions.Options;

namespace HQPlus.Task2.Repository {
    
    public class HotelRateJsonFileRepository : IHotelRateRepository {

        private readonly FileDataSourceOptions _dataSource;

        public HotelRateJsonFileRepository(IOptions<FileDataSourceOptions> options) {
            _dataSource = options.Value;
        }

        public async IAsyncEnumerable<HotelRate> GetAllAsync() {
            await using var stream = new FileStream(_dataSource.FilePath, FileMode.Open, FileAccess.Read);
            using var jsonDoc = await JsonDocument.ParseAsync(stream);
            var rootArr = jsonDoc.RootElement.EnumerateArray();
            var jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            foreach (var element in rootArr) {
                var hotelRateArr = element.GetProperty(nameof(HotelWithRates.HotelRates).ToCamelCase())
                    .EnumerateArray();

                foreach (var hotelRateElement in hotelRateArr) {
                    var hotelRate = hotelRateElement.ToObject<HotelRate>(jsonOptions);
                    yield return hotelRate;
                }
            }
        }
    }
    
}