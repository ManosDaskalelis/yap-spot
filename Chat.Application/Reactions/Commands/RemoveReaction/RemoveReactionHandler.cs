using Chat.Application.Abstractions;
using Chat.Application.Common.Exceptions;
using Chat.Contracts.Reactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Chat.Application.Reactions.Commands.RemoveReaction
{
    public sealed class RemoveReactionHandler : IRequestHandler<RemoveReactionCommand, ReactionRemovedDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public RemoveReactionHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<ReactionRemovedDto> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);
            var userId = _currentUserService.UserId;

            if (message == null)
            {
                throw new NotFoundException("There is no message to react to");
            }

            var isMember = await _dbContext.RoomMembers.AsNoTracking().AnyAsync(x => x.RoomId == message.RoomId && x.UserId == userId, cancellationToken);

            if (!isMember)
            {
                throw new ForbiddenException("You are not a member of this room");
            }

            var alreadyReacted = await _dbContext.MessageReactions.FirstOrDefaultAsync(x => x.UserId == userId && x.MessageId == request.MessageId && x.Emoji == request.Emoji.Trim(), cancellationToken);

            if (alreadyReacted == null)
            {
                throw new ConflictException("No reaction to remove");
            }

            message.EditedAtUtc = DateTime.UtcNow;

            _dbContext.MessageReactions.Remove(alreadyReacted);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ReactionRemovedDto(
                alreadyReacted.MessageId,
                message.RoomId,
                alreadyReacted.UserId,
                alreadyReacted.Emoji
            );

        }
    }
}
