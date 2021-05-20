using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using FluentAssertions;
using HQPlus.Data.Model;
using HQPlus.Task2.Mapper;
using HQPlus.Task2.Options;
using HQPlus.Task2.Report;
using HQPlus.Task2.Repository;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HQPlus.Task2.Tests.IntegrationTests {
    public class ExcelReportGeneratorSpecs {

        [Fact]
        public async Task GenerateReportAsync_gives_excel_with_data_from_hotel_rates_repository() {
            var hotelRatesRepoMock = new Mock<IHotelRateRepository>();
            hotelRatesRepoMock.Setup(m => m.GetAllAsync())
                .Returns(GetTestAsyncEnumerable());
            const string excelFilePath = "./testexcel.xlsx";
            var excelOptions = new ExcelSettingsOptions {
                OutputLocation = excelFilePath,
                SheetName = "test"
            };
            var excelOptionsMock = new Mock<IOptions<ExcelSettingsOptions>>();
            excelOptionsMock.Setup(m => m.Value)
                .Returns(excelOptions);
            var excelReportGenerator = new ExcelReportGenerator(hotelRatesRepoMock.Object, new HotelRateMapper(), excelOptionsMock.Object);

            await excelReportGenerator.GenerateReportAsync();

            hotelRatesRepoMock.Verify(m => m.GetAllAsync(), Times.Once);
            File.Exists(excelFilePath).Should().Be(true);
            AssertExcelFile(excelFilePath);
        }

        private void AssertExcelFile(string excelFilePath) {
            var workbook = new XLWorkbook(excelFilePath);
            var worksheet = workbook.Worksheet("test");
            worksheet.Cell(2, 1).Value.Should().Be(new DateTime(2016, 03, 15));
            worksheet.Cell(2, 2).Value.Should().Be(new DateTime(2016, 03, 16));
            worksheet.Cell(2, 3).Value.Should().Be(113.45M);
            worksheet.Cell(2, 4).Value.Should().Be("EUR");
            worksheet.Cell(2, 5).Value.Should().Be("Fake Hotel Rate Name");
            worksheet.Cell(2, 6).Value.Should().Be(2);
            worksheet.Cell(2, 7).Value.Should().Be(1);
        }

        private async IAsyncEnumerable<HotelRate> GetTestAsyncEnumerable() {
            yield return new HotelRate(
                "fakeId", 
                2,
                new Price("EUR", 113.45M, 11346),
                1,
                "fake rate description",
                "Fake Hotel Rate Name",
                new RateTag[] { new RateTag("breakfast", true) },
                DateTimeOffset.Parse("2016-03-15T00:00:00.000+01:00"));
        }

    }
}