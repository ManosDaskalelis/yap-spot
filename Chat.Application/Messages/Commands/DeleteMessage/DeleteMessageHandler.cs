using Chat.Application.Abstractions;
using Chat.Application.Common.Exceptions;
using Chat.Contracts.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Messages.Commands.DeleteMessage
{
    public sealed class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, MessageDeleteDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteMessageHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }


        public async Task<MessageDeleteDto> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var message = await _dbContext.Messages
                .FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);

            if (message is null)
                throw new NotFoundException("Message not found.");

            if (message.DeletedAtUtc is not null)
                throw new ValidationException("Message is already deleted.");

            if (message.SenderId != userId)
                throw new ForbiddenException("You can only delete your own messages.");

            var now = DateTime.UtcNow;

            message.DeletedAtUtc = now;
            message.Content = string.Empty;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new MessageDeleteDto(
                message.Id,
                message.RoomId,
                now
            );
        }
    }
}
