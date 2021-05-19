using System.Threading.Tasks;
using HQPlus.Task2.Report;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HQPlus.Task2.Tests.IntegrationTests {
    public class WorkerSpecs {

        [Fact]
        public async Task Worker_service_calls_report_generator() {
            var reportGeneratorMock = new Mock<IReportGenerator>();
            reportGeneratorMock.Setup(m => m.GenerateReportAsync());
            var loggerMock = new Mock<ILogger<Worker>>();
            IHostedService worker = new Worker(reportGeneratorMock.Object, loggerMock.Object);

            await worker.StartAsync(default);

            reportGeneratorMock.Verify(m => m.GenerateReportAsync(), Times.Once);
            await worker.StopAsync(default);
        }
        
    }
}