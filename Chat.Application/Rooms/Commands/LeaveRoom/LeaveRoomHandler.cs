using Chat.Application.Abstractions;
using Chat.Application.Common.Exceptions;
using Chat.Contracts.Rooms;
using Chat.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Commands.LeaveRoom
{
    public sealed class LeaveRoomHandler : IRequestHandler<LeaveRoomCommand, LeaveRoomResult>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _curentUser;

        public LeaveRoomHandler(ICurrentUserService curentUser, IApplicationDbContext dbContext)
        {
            _curentUser = curentUser;
            _dbContext = dbContext;
        }
        public async Task<LeaveRoomResult> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
        {
            var roomExists = await _dbContext.Rooms.AsNoTracking().AnyAsync(x => x.Id == request.RoomId, cancellationToken);

            if (!roomExists)
            {
                throw new NotFoundException("Room not found");
            }

            var userId = _curentUser.UserId;

            var membership = await _dbContext.RoomMembers.FirstOrDefaultAsync(x => x.RoomId == request.RoomId && x.UserId == userId, cancellationToken);

            if (membership == null)
            {
                throw new ForbiddenException("You are not a member of this room");
            }

            if (membership.Role == RoomMemberRoleEnum.Owner)
            {
                throw new ConflictException("The owner cannot leave the room. Delete it instead");
            }

            try
            {
                _dbContext.RoomMembers.Remove(membership);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (ConflictException ex)
            {
                throw;
            }

            return new LeaveRoomResult(request.RoomId, userId, DateTime.UtcNow);

        }
    }
}
