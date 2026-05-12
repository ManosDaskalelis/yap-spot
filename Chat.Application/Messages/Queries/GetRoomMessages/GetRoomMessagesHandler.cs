using Chat.Application.Abstractions;
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
    public sealed class GetRoomMessagesHandler : IRequestHandler<GetRoomMessagesQuery, List<string>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetRoomMessagesHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<List<string>> Handle(GetRoomMessagesQuery request, CancellationToken cancellationToken)
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
                throw new ArgumentException("You are not a member of this room");
            }

            var messages = new List<string>();
            foreach (var msg in _dbContext.Messages.AsNoTracking())
            {
                messages.Add($"User: {userId} said: {msg.Content}      {msg.CreatedAtUtc}");
            }

            return messages;
        }
    }
}
