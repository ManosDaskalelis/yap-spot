using Chat.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Reactions.Commands.RemoveReaction
{
    public sealed class RemoveReactionHandler : IRequestHandler<RemoveReactionCommand, bool>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public RemoveReactionHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);
            var userId = _currentUserService.UserId;

            if (message == null)
            {
                throw new ArgumentException("There is no message to react to");
            }

            var isMember = await _dbContext.RoomMembers.AsNoTracking().AnyAsync(x => x.RoomId == message.RoomId && x.UserId == userId, cancellationToken);

            if (!isMember)
            {
                throw new UnauthorizedAccessException("You are not a member of this room");
            }

            var alreadyReacted = await _dbContext.MessageReactions.FirstOrDefaultAsync(x => x.UserId == userId && x.MessageId == request.MessageId && x.Emoji == request.Emoji.Trim(), cancellationToken);

            if (alreadyReacted == null)
            {
                throw new InvalidOperationException("No reaction to remove");
            }

            message.EditedAtUtc = DateTime.UtcNow;

            _dbContext.MessageReactions.Remove(alreadyReacted);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
