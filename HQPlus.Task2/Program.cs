using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HQPlus.Task2.Mapper;
using HQPlus.Task2.Options;
using HQPlus.Task2.Report;
using HQPlus.Task2.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HQPlus.Task2 {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {
                    var configuration = hostContext.Configuration;
                    services.Configure<FileDataSourceOptions>(configuration.GetSection(FileDataSourceOptions.SectionName));
                    services.Configure<ExcelSettingsOptions>(configuration.GetSection(ExcelSettingsOptions.SectionName));
                    services.AddSingleton<IHotelRateRepository, HotelRateJsonFileRepository>();
                    services.AddSingleton<HotelRateMapper>();
                    services.AddSingleton<IReportGenerator, ExcelReportGenerator>();
                    services.AddHostedService<Worker>();
                });
    }
}