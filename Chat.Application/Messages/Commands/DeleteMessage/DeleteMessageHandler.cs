using Chat.Application.Abstractions;
using Chat.Contracts.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new InvalidOperationException("Message not found.");

            if (message.DeletedAtUtc is not null)
                throw new InvalidOperationException("Message is already deleted.");

            if (message.SenderId != userId)
                throw new UnauthorizedAccessException("You can only delete your own messages.");

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
