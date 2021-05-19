using System;
using FluentAssertions;
using HQPlus.Data.Model;
using HQPlus.Task2.Mapper;
using Xunit;

namespace HQPlus.Task2.Tests.UnitTests {
    public class HotelRateMapperSpecs {

        [Fact]
        public void HotelRate_targetday_date_is_excel_summary_arrival_date() {
            var hotelRate = GetFakeHotelRate(false);
            var mapper = new HotelRateMapper();
            var expectedDate = hotelRate.TargetDay.Date;

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.ArrivalDate.Should().Be(expectedDate);
        }

        [Fact]
        public void Excel_summary_departure_date_is_hotelrate_targetday_plus_los() {
            var hotelRate = GetFakeHotelRate(false);
            var mapper = new HotelRateMapper();
            var expectedDepartureDate = hotelRate.TargetDay.Date.AddDays(hotelRate.Los).Date;

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.DepartureDate.Should().Be(expectedDepartureDate);
        }

        [Fact]
        public void Excel_summary_price_is_hotelrate_price_numeric_float() {
            var hotelRate = GetFakeHotelRate(false);
            var mapper = new HotelRateMapper();
            var expectedPrice = hotelRate.Price.NumericFloat;

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.Price.Should().Be(expectedPrice);
        }

        [Fact]
        public void HotelRate_price_currency_is_excel_summary_currency() {
            var hotelRate = GetFakeHotelRate(false);
            var mapper = new HotelRateMapper();

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.Currency.Should().Be(hotelRate.Price.Currency);
        }

        [Fact]
        public void HotelRate_rate_name_is_excel_summary_rate_name() {
            var hotelRate = GetFakeHotelRate(false);
            var mapper = new HotelRateMapper();

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.RateName.Should().Be(hotelRate.RateName);
        }

        [Fact]
        public void HotelRate_adults_is_excel_summary_adults() {
            var hotelRate = GetFakeHotelRate(false);
            var mapper = new HotelRateMapper();

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.Adults.Should().Be(hotelRate.Adults);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Excel_summary_breakfast_is_hotelrate_breakfast_rate_tag_shape(bool breakfastIncluded) {
            var hotelRate = GetFakeHotelRate(breakfastIncluded);
            var mapper = new HotelRateMapper();
            var expectedBreakfastIncluded = breakfastIncluded ? 1 : 0;

            var excelSummary = mapper.MapHotelRateToExcelSummary(hotelRate);

            excelSummary.BreakfastIncluded.Should().Be(expectedBreakfastIncluded);
        }

        private HotelRate GetFakeHotelRate(bool includeBreakfast) {
            var price = new Price(
                "EUR",
                131.956M,
                13110);
            var dateTimeOffset = DateTimeOffset.Parse("2016-03-15T00:00:00.000+01:00");

            return new HotelRate(
                "1",
                2,
                price,
                1,
                "fake description 1",
                "fake rate name",
                GetRateTags(includeBreakfast),
                dateTimeOffset);
        }

        private static RateTag[] GetRateTags(bool includeBreakfast) {
            return new RateTag[] {
                new RateTag(
                    "breakfast",
                    includeBreakfast)
            };
        }
    }
}