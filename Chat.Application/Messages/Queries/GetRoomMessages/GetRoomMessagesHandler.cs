using Chat.Application.Abstractions;
using Chat.Contracts.Messages;
using Chat.Contracts.Reactions;
using Chat.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Messages.Queries.GetRoomMessages
{
    public sealed class GetRoomMessagesHandler : IRequestHandler<GetRoomMessagesQuery, IReadOnlyList<MessageDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetRoomMessagesHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<MessageDto>> Handle(GetRoomMessagesQuery request, CancellationToken cancellationToken)
        {
            var roomExists = await _dbContext.Rooms.AsNoTracking().AnyAsync(x => x.Id == request.RoomId, cancellationToken);
            var userId = _currentUserService.UserId;

            if (!roomExists)
            {
                throw new ArgumentException("Room not found");
            }

            var isMember = await _dbContext.RoomMembers.AsNoTracking().AnyAsync(x => x.UserId == userId && x.RoomId == request.RoomId, cancellationToken);

            if (!isMember)
            {
                throw new UnauthorizedAccessException("You are not a member of this room.");
            }

            var messages = await _dbContext.Messages
                .AsNoTracking()
                .Where(x => x.RoomId == request.RoomId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new
                {
                    x.Id,
                    x.RoomId,
                    x.SenderId,
                    x.Content,
                    x.Type,
                    x.CreatedAtUtc,
                    x.EditedAtUtc,
                    x.DeletedAtUtc
                }).ToListAsync(cancellationToken);

            var messageIds = messages
                .Select(x => x.Id)
                .ToList();

            var reactions = await _dbContext.MessageReactions
                .AsNoTracking()
                .Where(x => messageIds.Contains(x.MessageId))
                .ToListAsync(cancellationToken);

            var reactionsByMessage = reactions
           .GroupBy(x => x.MessageId)
           .ToDictionary(
               messageGroup => messageGroup.Key,
               messageGroup => messageGroup
                   .GroupBy(x => x.Emoji)
                   .Select(emojiGroup => new MessageReactionSummaryDto(
                       Emoji: emojiGroup.Key,
                       Count: emojiGroup.Count(),
                       ReactedByCurrentUser: emojiGroup.Any(x => x.UserId == userId)
                   ))
                   .ToList()
           );

            return messages
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new MessageDto(Id: x.Id, RoomId: x.RoomId, SenderId: x.SenderId, Content: x.DeletedAtUtc is null ? x.Content : "This message was deleted", Type: x.Type.ToString(), CreatedAtUtc: x.CreatedAtUtc, Reactions: reactionsByMessage.TryGetValue(x.Id, out var messageReactions) ? messageReactions : [])).ToList();
        }
    }
}
