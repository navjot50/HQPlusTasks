using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using HQPlus.Data.Extensions;
using HQPlus.Data.Model;
using HQPlus.Task3.Api.Options;
using Microsoft.Extensions.Options;

namespace HQPlus.Task3.Api.Repository {
    public sealed class HotelWithRatesJsonFileRepository : IHotelWithRatesRepository {

        private readonly FileDataSourceOptions _dataSource;

        private static readonly JsonSerializerOptions CaseInsensitiveProperty = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        public HotelWithRatesJsonFileRepository(IOptionsSnapshot<FileDataSourceOptions> dataSource) {
            _dataSource = dataSource.Value;
        }

        public async Task<HotelWithRates> GetAsync(long id, DateTime arrivalDate) {
            await using var jsonStream = new FileStream(_dataSource.FilePath, FileMode.Open, FileAccess.Read);
            using var jsonDoc = await JsonDocument.ParseAsync(jsonStream);
            var rootArr = jsonDoc.RootElement.EnumerateArray();

            HotelWithRates hotelWithRates = null;
            foreach (var element in rootArr) {
                var hotelElement = element.GetProperty(nameof(HotelWithRates.Hotel).ToCamelCase());
                var hotelId = hotelElement.GetProperty(nameof(Hotel.HotelID).ToCamelCase())
                    .GetInt64();

                if (hotelId != id) {
                    continue;
                }

                var hotel = hotelElement.ToObject<Hotel>(CaseInsensitiveProperty);
                var hotelRatesArr = element.GetProperty(nameof(HotelWithRates.HotelRates).ToCamelCase())
                    .EnumerateArray();
                var hotelRatesForDate = GetHotelRatesForDate(hotelRatesArr, arrivalDate);
                hotelWithRates = new HotelWithRates(hotel, hotelRatesForDate);
                break;
            }

            return hotelWithRates;
        }

        private HotelRate[] GetHotelRatesForDate(IEnumerable<JsonElement> hotelRatesArr, DateTime arrivalDate) {
            var hotelRatesForDate = new List<HotelRate>();
            
            foreach (var hotelRateElement in hotelRatesArr) {
                var targetDay = hotelRateElement.GetProperty(nameof(HotelRate.TargetDay).ToCamelCase())
                    .GetDateTimeOffset();
                if (targetDay.Date != arrivalDate.Date) {
                    continue;
                }

                var hotelRate = hotelRateElement.ToObject<HotelRate>(CaseInsensitiveProperty);
                hotelRatesForDate.Add(hotelRate);
            }

            return hotelRatesForDate.ToArray();
        }
    }
}