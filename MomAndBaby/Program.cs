using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MomAndBaby.API;
using MomAndBaby.Repositories;
using MomAndBaby.Repositories.ConfigContext;
using MomAndBaby.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Docker
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        builder.WebHost.UseUrls($"http://*:{port}");

        // Add services to the container.

        #region MyConfiguration

        //DI 3 layers
        builder.Services.AddConfigurationAPI(builder.Configuration);
        builder.Services.AddConfigServices(builder.Configuration);
        builder.Services.AddConfigurationRepositories();

        //Config appsettings when CICD
        builder.Configuration
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();

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
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        // Configure the database context
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MBContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MomAndBaby API V1");
                c.RoutePrefix = string.Empty; // Hiển thị Swagger tại `/`
            });

        app.UseHealthChecks("/health");

        app.UseCors("AllowSpecificOrigins");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

