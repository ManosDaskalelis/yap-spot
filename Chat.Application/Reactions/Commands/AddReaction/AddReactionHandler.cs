using Chat.Application.Abstractions;
using Chat.Application.Common.Emojis;
using Chat.Application.Common.Exceptions;
using Chat.Contracts.Reactions;
using Chat.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Reactions.Commands.AddReaction
{
    public sealed class AddReactionHandler : IRequestHandler<AddReactionCommand, ReactionDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public AddReactionHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<ReactionDto> Handle(AddReactionCommand request, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);
            var userId = _currentUserService.UserId;

            if (message == null)
            {
                throw new NotFoundException("Message not found");
            }

            var isMember = await _dbContext.RoomMembers.AsNoTracking().AnyAsync(x => x.RoomId == message.RoomId && x.UserId == userId, cancellationToken);

            if (!isMember)
            {
                throw new ForbiddenException("You are not a member of this room");
            }

            var alreadyReacted = await _dbContext.MessageReactions.AsNoTracking().AnyAsync(x => x.UserId == userId && x.MessageId == request.MessageId && x.Emoji == request.Emoji.Trim(), cancellationToken);

            if (alreadyReacted)
            {
                throw new ConflictException("You already reacted with this emoji.");
            }

            var reaction = new MessageReaction
            {
                UserId = userId,
                MessageId = request.MessageId,
                Emoji = EmojiNormalizer.Normalize(request.Emoji),
                CreatedAtUtc = DateTime.UtcNow
            };

            message.EditedAtUtc = DateTime.UtcNow;

            _dbContext.MessageReactions.Add(reaction);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ReactionDto(
                reaction.MessageId,
                message.RoomId,
                reaction.UserId,
                reaction.Emoji,
                reaction.CreatedAtUtc
            );

        }
    }
}
