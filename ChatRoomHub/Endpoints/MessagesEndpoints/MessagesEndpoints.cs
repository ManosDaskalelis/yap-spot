using Chat.Application.Messages.Commands.DeleteMessage;
using Chat.Application.Messages.Commands.EditMessage;
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

            group.MapPost("/{roomId:guid}", async (Guid roomId, SendMessageRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new SendMessageCommand(roomId, request.content), ct);
                return Results.Ok(result);
            });

            group.MapGet("/{roomId:guid}", async (Guid roomId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetRoomMessagesQuery(roomId), ct);
                return Results.Ok(result);
            });

            group.MapPut("/{messageId:guid}", async (Guid messageId, EditMessageRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new EditMessageCommand(messageId, request.Content), ct);
                return Results.Ok(result);
            });

            group.MapDelete("/{messageId:guid}", async (Guid messageId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteMessageCommand(messageId), ct);
                return Results.Ok(result);
            });
        }
    }
}
