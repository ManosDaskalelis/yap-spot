using Chat.Application.Reactions.Commands.AddReaction;
using Chat.Application.Reactions.Commands.RemoveReaction;
using Chat.Contracts.Messages;
using Chat.Contracts.Reactions;
using MediatR;

namespace Chat.Api.Endpoints.ReactionEndpoints
{
    public static class ReactionEndpoints
    {
        public static void MapReactionEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/reactions")
                .WithTags("Reactions");

            group.MapPost("/{messageId:guid}", async (Guid messageId, AddReactionRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new AddReactionCommand(messageId, request.Emoji), ct);
                return Results.Ok(result);
            });

            group.MapDelete("/{messageId:guid}", async (Guid messageId, string emoji, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new RemoveReactionCommand(messageId, emoji), ct);
                return Results.Ok(result);
            });
        }
    }
}
