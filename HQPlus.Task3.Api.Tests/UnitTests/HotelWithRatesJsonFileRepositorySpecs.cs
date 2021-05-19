using System;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using HQPlus.Task3.Api.Options;
using HQPlus.Task3.Api.Repository;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HQPlus.Task3.Api.Tests.UnitTests {
    public class HotelWithRatesJsonFileRepositorySpecs {

        [Fact]
        public async Task Get_gives_hotel_for_given_id() {
            var fileDataSourceOptions = new FileDataSourceOptions {
                FilePath = "./testhotelsrates.json"
            };
            var fileDataSourceOptionsMock = new Mock<IOptionsSnapshot<FileDataSourceOptions>>();
            fileDataSourceOptionsMock.Setup(m => m.Value)
                .Returns(fileDataSourceOptions);
            IHotelWithRatesRepository repo = new HotelWithRatesJsonFileRepository(fileDataSourceOptionsMock.Object);
            const int id = 1;

            var hotelWithRates = await repo.GetAsync(id, DateTime.Now);

            hotelWithRates.Hotel.HotelID.Should().Be(id);
        }

        [Fact]
        public async Task Get_gives_hotel_for_given_id_and_date() {
            var fileDataSourceOptions = new FileDataSourceOptions {
                FilePath = "./testhotelsrates.json"
            };
            var fileDataSourceOptionsMock = new Mock<IOptionsSnapshot<FileDataSourceOptions>>();
            fileDataSourceOptionsMock.Setup(m => m.Value)
                .Returns(fileDataSourceOptions);
            IHotelWithRatesRepository repo = new HotelWithRatesJsonFileRepository(fileDataSourceOptionsMock.Object);
            const int id = 1;
            var arrivalDate = DateTime.ParseExact("2016-03-15", "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var hotelWithRates = await repo.GetAsync(id, arrivalDate);

            hotelWithRates.Hotel.HotelID.Should().Be(id);
            hotelWithRates.HotelRates.Should().HaveCount(1);
            hotelWithRates.HotelRates[0].TargetDay.Date.Should().Be(arrivalDate);
        }

        [Fact]
        public async Task Get_gives_empty_hotelrates_for_non_existing_date() {
            var fileDataSourceOptions = new FileDataSourceOptions {
                FilePath = "./testhotelsrates.json"
            };
            var fileDataSourceOptionsMock = new Mock<IOptionsSnapshot<FileDataSourceOptions>>();
            fileDataSourceOptionsMock.Setup(m => m.Value)
                .Returns(fileDataSourceOptions);
            IHotelWithRatesRepository repo = new HotelWithRatesJsonFileRepository(fileDataSourceOptionsMock.Object);
            const int id = 1;
            var arrivalDate = DateTime.ParseExact("2021-04-12", "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var hotelWithRates = await repo.GetAsync(id, arrivalDate);

            hotelWithRates.HotelRates.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_gives_null_for_non_existing_id() {
            var fileDataSourceOptions = new FileDataSourceOptions {
                FilePath = "./testhotelsrates.json"
            };
            var fileDataSourceOptionsMock = new Mock<IOptionsSnapshot<FileDataSourceOptions>>();
            fileDataSourceOptionsMock.Setup(m => m.Value)
                .Returns(fileDataSourceOptions);
            IHotelWithRatesRepository repo = new HotelWithRatesJsonFileRepository(fileDataSourceOptionsMock.Object);
            const int id = 50;

            var hotelWithRates = await repo.GetAsync(id, DateTime.Now);

            hotelWithRates.Should().BeNull();
        }
    }
}