using Chat.Application.Messages.Commands.SendMessage;
using Chat.Application.Messages.Queries.GetRoomMessages;
using Chat.Contracts.Messages;
using MediatR;

namespace Chat.Api.Endpoints.Messages
{
    public static class MessagesEndpoints
    {
        public static void MapMessagesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/messages")
                .WithTags("Messages");

            group.MapPost("/{roomId:guid}/messages", async (Guid roomId, SendMessageRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new SendMessageCommand(roomId, request.content), ct);
                return Results.Ok(result);
            });

            group.MapGet("/{roomId:guid}/messages", async (Guid roomId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetRoomMessagesQuery(roomId), ct);
                return Results.Ok(result);
            });
        }
    }
}
