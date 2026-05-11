using Chat.Application.Messages.Commands.SendMessage;
using Chat.Application.Rooms.Commands.CreateRoom;
using Chat.Application.Rooms.Commands.JoinRoom;
using Chat.Contracts.Messages;
using Chat.Contracts.Rooms;
using MediatR;

namespace Chat.Api.Endpoints
{
    public static class RoomsEndpoint
    {
        public static void MapRoomEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/rooms")
                .WithTags("Rooms");

            group.MapPost("/create-room", async (CreateRoomRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new CreateRoomCommand(request.RoomName, request.RoomType), ct);
                return Results.Ok(result);
            });

            group.MapPost("/{roomId:guid}/join", async (Guid roomId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new JoinRoomCommand(roomId), ct);
                return Results.Ok(result);
            });

            group.MapPost("/{roomId:guid}/messages", async (Guid roomId, SendMessageRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new SendMessageCommand(roomId, request.content), ct);
                return Results.Ok(result);
            });
        }
    }
}
