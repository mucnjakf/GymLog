using GymLog.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymLab.IntegrationTests;

public class TestingWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? descriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("GymLogTestDb"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope serviceScope = serviceProvider.CreateScope();

            using ApplicationDbContext appContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            appContext.Database.EnsureCreated();
        });
    }
}