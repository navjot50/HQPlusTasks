using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HQPlus.Data.Model;
using Xunit;

namespace HQPlus.Task3.Api.Tests.IntegrationTests {
    
    public class HotelsControllerApiSpecs : IClassFixture<TestServerFixture> {

        private readonly HttpClient _client;

        private static readonly JsonSerializerOptions CaseInsenstive = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        public HotelsControllerApiSpecs(TestServerFixture factory) {
            _client = factory.CreateClient();
        }

        [Fact]
        public void GetHotel_gives_badrequest_for_non_ISO_date() {
            const string nonISODateQueryParam = "03-02-15";
            const string uri = "/api/hotels/1?arrivalDate=" + nonISODateQueryParam;

            var response = _client.GetAsync(uri).Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void GetHotel_gives_notfound_for_non_existing_hotel_id() {
            const long hotelId = 50;
            const string arrivalDate = "2016-03-15";
            var uri = "/api/hotels/" + hotelId + "?arrivalDate=" + arrivalDate;

            var response = _client.GetAsync(uri).Result;

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetHotel_gives_hotel_for_existing_hotel_id() {
            const long hotelId = 1;
            const string arrivalDate = "2016-03-15";
            var uri = "/api/hotels/" + hotelId + "?arrivalDate=" + arrivalDate;

            var response = _client.GetAsync(uri).Result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseJson = await response.Content.ReadAsStringAsync();
            var hotelWithRates = JsonSerializer.Deserialize<HotelWithRates>(responseJson, CaseInsenstive);
            hotelWithRates.Hotel.HotelID.Should().Be(1);
        }

        [Fact]
        public async Task GetHotel_gives_hotelrates_for_given_ISO_date() {
            const long hotelId = 1;
            const string arrivalDate = "2016-03-15";
            var uri = "/api/hotels/" + hotelId + "?arrivalDate=" + arrivalDate;
            var expectedDate = DateTime.ParseExact(arrivalDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var response = _client.GetAsync(uri).Result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseJson = await response.Content.ReadAsStringAsync();
            var hotelWithRates = JsonSerializer.Deserialize<HotelWithRates>(responseJson, CaseInsenstive);
            hotelWithRates.HotelRates.Should().HaveCount(1);
            hotelWithRates.HotelRates[0].TargetDay.Date.Should().Be(expectedDate);
        }

    }
}