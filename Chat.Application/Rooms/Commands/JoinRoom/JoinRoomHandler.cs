using Chat.Application.Abstractions;
using Chat.Contracts.Rooms;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Commands.JoinRoom
{
    public sealed class JoinRoomHandler : IRequestHandler<JoinRoomCommand, JoinRoomResult>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _curentUser;

        public JoinRoomHandler(ICurrentUserService curentUser, IApplicationDbContext dbContext)
        {
            _curentUser = curentUser;
            _dbContext = dbContext;
        }

        public async Task<JoinRoomResult> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
        {
            var userId = _curentUser.UserId;

            var roomExists = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == request.RoomId, cancellationToken);

            if (roomExists == null)
            {
                throw new Exception("Room not found");
            }

            var alreadyJoined = await _dbContext.RoomMembers.AnyAsync(x => x.UserId == userId && x.RoomId == request.RoomId, cancellationToken);

            if (alreadyJoined)
            {
                throw new Exception("Already joined");
            }

            var member = new RoomMember
            {
                RoomId = request.RoomId,
                UserId = userId,
                Role = RoomMemberRoleEnum.Member,
                JoinedAtUtc = DateTime.UtcNow
            };

            _dbContext.RoomMembers.Add(member);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new JoinRoomResult(request.RoomId, userId, member.Role.ToString(), member.JoinedAtUtc);

        }
    }
}
