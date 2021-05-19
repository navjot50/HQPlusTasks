using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HQPlus.Task2.Report;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HQPlus.Task2 {
    public class Worker : BackgroundService {
        
        private readonly ILogger<Worker> _logger;

        private readonly IReportGenerator _reportGenerator;

        public Worker(IReportGenerator reportGenerator, ILogger<Worker> logger) {
            _reportGenerator = reportGenerator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            try {
                await _reportGenerator.GenerateReportAsync();
                _logger.LogInformation("Hotel Rates Excel report generated");
            }
            catch (Exception ex) {
                _logger.LogError(new EventId(ex.HResult), ex, ex.Message);
            }
        }
    }
}