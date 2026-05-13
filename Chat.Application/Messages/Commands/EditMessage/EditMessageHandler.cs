using Chat.Application.Abstractions;
using Chat.Contracts.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Messages.Commands.EditMessage
{
    public sealed class EditMessageHandler : IRequestHandler<EditMessageCommand, MessageEditDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public EditMessageHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }


        public async Task<MessageEditDto> Handle(EditMessageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                throw new ArgumentException("Message content cannot be empty");
            }

            var userId = _currentUserService.UserId;

            var message = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);

            if (message is null)
                throw new InvalidOperationException("Message not found.");

            if (message.DeletedAtUtc is not null)
                throw new InvalidOperationException("Cannot edit a deleted message.");

            if (message.SenderId != userId)
                throw new UnauthorizedAccessException("You can only edit your own messages.");

            message.Content = request.Content;
            message.EditedAtUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new MessageEditDto(message.Id, message.RoomId, message.Content, message.EditedAtUtc.Value);
        }
    }
}
