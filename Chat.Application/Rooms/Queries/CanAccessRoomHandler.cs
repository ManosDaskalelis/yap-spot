using Chat.Application.Abstractions;
using Chat.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Queries
{
    public sealed class CanAccessRoomHandler : IRequestHandler<CanAccessRoomQuery, bool>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CanAccessRoomHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }


        public async Task<bool> Handle(CanAccessRoomQuery request, CancellationToken cancellationToken)
        {
            var roomExists = await _dbContext.Rooms.AsNoTracking().AnyAsync(x => x.Id == request.RoomId);
            var userId = _currentUserService.UserId;

            if (!roomExists)
            {
                throw new ArgumentException("Room not found");
            }

            var isMember = await _dbContext.RoomMembers.AnyAsync(x => x.RoomId == request.RoomId && x.UserId == userId, cancellationToken);

            if (!isMember)
            {
                throw new ArgumentException("You are not a member of this room");
            }

            return true;
        }
    }
}
