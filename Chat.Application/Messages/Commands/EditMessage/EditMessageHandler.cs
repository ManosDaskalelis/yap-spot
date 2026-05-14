using Chat.Application.Abstractions;
using Chat.Application.Common.Exceptions;
using Chat.Contracts.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
                throw new Common.Exceptions.ValidationException("Message content cannot be empty");
            }

            var userId = _currentUserService.UserId;

            var message = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);

            if (message is null)
                throw new NotFoundException("Message not found.");

            if (message.DeletedAtUtc is not null)
                throw new Common.Exceptions.ValidationException("Cannot edit a deleted message.");

            if (message.SenderId != userId)
                throw new ForbiddenException("You can only edit your own messages.");

            message.Content = request.Content;
            message.EditedAtUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new MessageEditDto(message.Id, message.RoomId, message.Content, message.EditedAtUtc.Value);
        }
    }
}
