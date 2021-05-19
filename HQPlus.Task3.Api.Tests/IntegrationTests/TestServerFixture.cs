using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace HQPlus.Task3.Api.Tests.IntegrationTests {
    public class TestServerFixture : WebApplicationFactory<Startup> {
        
        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            var path = Assembly.GetAssembly(typeof(TestServerFixture))
                ?.Location;

            builder.UseContentRoot(Path.GetDirectoryName(path) ?? throw new InvalidOperationException())
                .ConfigureAppConfiguration(cb => {
                    cb.AddJsonFile("appsettings.json", false)
                        .AddEnvironmentVariables();
                });
        }
        
    }
}