using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Rooms
{
    public sealed record JoinRoomResult(Guid RoomId, Guid UserId, string Role, DateTime JoinedAtUtc);
}
