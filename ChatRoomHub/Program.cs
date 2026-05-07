using Chat.Api.Endpoints;
using Chat.Application;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Infrastructure;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //var summaries = new[]
            //{
            //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            //};

            //app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            //{
            //    var forecast = Enumerable.Range(1, 5).Select(index =>
            //        new WeatherForecast
            //        {
            //            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //            TemperatureC = Random.Shared.Next(-20, 55),
            //            Summary = summaries[Random.Shared.Next(summaries.Length)]
            //        })
            //        .ToArray();
            //    return forecast;
            //})
            //.WithName("GetWeatherForecast");

            app.MapGet("/dev/seed-user", async (ChatDbContext db) =>
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

                var exist = await db.Users.AnyAsync(x => x.Id == userId);

                if (exist) return Results.Ok("User already in db");

                var user = new User
                {
                    Id = userId,
                    AuthUserId = "fake-auth-user-1",
                    Username = "test-user",
                    DisplayName = "Test User",
                    AvatarUrl = null,
                    Role = UserRoleEnum.User,
                    ActivityStatus = UserStatusEnum.Online,
                    CreatedAtUtc = DateTime.UtcNow
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return Results.Ok("Fake User Created");
            });

            app.MapRoomEndpoints();

            app.Run();
        }
    }
}
