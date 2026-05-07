using Chat.Application.Abstractions;
using Chat.Contracts.Rooms;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Commands.CreateRoom
{
    public sealed class CreateRoomHandler : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public CreateRoomHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            Enum.TryParse<RoomTypeEnum>(request.RoomType, ignoreCase: true, out var roomType);

            var room = new Room
            {
                Id = Guid.NewGuid(),
                RoomName = request.RoomName,
                Type = roomType,
                CreatedByUserId = userId,
                CreatedAtUtc = DateTime.UtcNow
            };

            var member = new RoomMember
            {
                RoomId = room.Id,
                UserId = userId,
                Role = RoomMemberRoleEnum.Owner,
                JoinedAtUtc = DateTime.UtcNow
            };

            _dbContext.Rooms.Add(room);
            _dbContext.RoomMembers.Add(member);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RoomDto(
                room.Id,
                room.RoomName,
                room.Type.ToString(),
                room.CreatedAtUtc
                );
        }
    }
}
