using System.Linq;
using HQPlus.Data.Model;
using HQPlus.Task2.Report;

namespace HQPlus.Task2.Mapper {
    public sealed class HotelRateMapper {

        private const string Breakfastliteral = "breakfast";

        public HotelRateExcelRow MapHotelRateToExcelSummary(HotelRate hotelRate) {
            var arrivalDate = hotelRate.TargetDay.Date;
            var departureDate = arrivalDate.AddDays(hotelRate.Los).Date;
            var price = hotelRate.Price.NumericFloat;
            var currency = hotelRate.Price.Currency;
            var rateName = hotelRate.RateName;
            var adults = hotelRate.Adults;
            var breakfastIncluded = hotelRate.RateTags
                .Any(tag => tag.Name == Breakfastliteral && tag.Shape);
            var breakfastIncludedAsInt = breakfastIncluded ? 1 : 0;
            
            return new HotelRateExcelRow(
                arrivalDate, 
                departureDate, 
                price, 
                currency, 
                rateName, 
                adults, 
                breakfastIncludedAsInt);
        }

    }
}