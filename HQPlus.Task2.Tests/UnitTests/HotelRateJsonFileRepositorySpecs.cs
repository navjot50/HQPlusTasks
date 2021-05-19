using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HQPlus.Task2.Options;
using HQPlus.Task2.Repository;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HQPlus.Task2.Tests.UnitTests {
    public class HotelRateJsonFileRepositorySpecs {

        [Fact]
        public async Task GetAll_gives_all_hotelrates() {
            var dataSourceOptions = new FileDataSourceOptions {
                FilePath = "./testhotelsrates.json"
            };
            var fileOptionsMock = new Mock<IOptions<FileDataSourceOptions>>();
            fileOptionsMock.Setup(m => m.Value)
                .Returns(dataSourceOptions);
            IHotelRateRepository repository = new HotelRateJsonFileRepository(fileOptionsMock.Object);

            var hotelRatesResult = repository.GetAllAsync();

            var hotelRateIdsList = new List<string>();
            await foreach (var hotelRate in hotelRatesResult)
                hotelRateIdsList.Add(hotelRate.RateID);

            hotelRateIdsList.Should().Equal(new List<string> { "1", "2", "1", "2" });
        }
        
    }
}