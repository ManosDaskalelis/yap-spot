using Chat.Domain.Enums;
using System;

namespace Chat.Application.Abstractions
{
    public interface IRoomAuthorizationService
    {
        Task<bool> IsRoomMemberAsync(Guid roomId, Guid userId, CancellationToken ct);
        Task<bool> HasAnyRoleAsync(Guid roomId, Guid userId, RoomMemberRoleEnum[] roles, CancellationToken ct);
    }
}
