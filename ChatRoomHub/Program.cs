using Chat.Api.Endpoints;
using Chat.Api.Endpoints.Messages;
using Chat.Api.Endpoints.RoomsEndpoints;
using Chat.Api.Hubs;
using Chat.Application;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Infrastructure;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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
            builder.Services.AddSignalR();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ClientPolicy", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
                app.MapGet("/", () => Results.Redirect("/scalar")).WithOpenApi().WithTags("Scalar Endpoint");
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

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
            }).WithOpenApi().WithTags("Seed user");

            app.UseCors("ClientPolicy");
            app.MapHub<ChatHub>("/hubs/chat");
            app.MapRoomEndpoints();
            app.MapMessagesEndpoints();
            app.MapReactionEndpoints();

            app.Run();
        }
    }
}
