using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MomAndBaby.API;
using MomAndBaby.Repositories;
using MomAndBaby.Repositories.ConfigContext;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Services;
using MomAndBaby.Services.HubRealTime;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();
        });

        // Add services to the container.

        #region MyConfiguration

        //DI 3 layers
        builder.Services.AddConfigurationAPI(builder.Configuration);
        builder.Services.AddConfigServices(builder.Configuration);
        builder.Services.AddConfigurationRepositories();

        //Config appsettings when CICD
        //builder.Configuration
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //                .AddEnvironmentVariables();

        #endregion


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                            });

        var app = builder.Build();

        // Configure the database context
        //using (var scope = app.Services.CreateScope())
        //{
        //    var dbContext = scope.ServiceProvider.GetRequiredService<MBContext>();
        //    dbContext.Database.Migrate();
        //}

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<ChatHubR>("/chathub", options =>
        {
            options.Transports =
                Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets |
                Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents |
                Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
        });


        app.Run();
    }
}

