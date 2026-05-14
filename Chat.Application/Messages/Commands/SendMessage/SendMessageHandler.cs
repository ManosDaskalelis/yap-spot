using Chat.Application.Abstractions;
using Chat.Application.Common.Exceptions;
using Chat.Contracts.Messages;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Messages.Commands.SendMessage
{
    public sealed class SendMessageHandler : IRequestHandler<SendMessageCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public SendMessageHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            if (request.Content == null)
            {
                throw new NotFoundException("Message cannot be null");
            }

            var userId = _currentUserService.UserId;

            var roomExists = await _dbContext.Rooms.AnyAsync(x => x.Id == request.RoomId, cancellationToken);

            if (!roomExists)
            {
                throw new NotFoundException("Room not found");
            }

            var isMember = await _dbContext.RoomMembers.AnyAsync(x => x.RoomId == request.RoomId && x.UserId == userId, cancellationToken);

            if (!isMember)
            {
                throw new ForbiddenException("You are not a member of this room.");
            }

            var message = new Message
            {
                Id = new Guid(),
                RoomId = request.RoomId,
                SenderId = userId,
                Content = request.Content,
                Type = MessageTypeEnum.Text,
                CreatedAtUtc = DateTime.UtcNow
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return message.Id;

        }
    }
}
