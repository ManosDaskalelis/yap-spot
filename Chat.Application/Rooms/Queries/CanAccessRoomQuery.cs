using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Queries
{
    public sealed record CanAccessRoomQuery(Guid RoomId): IRequest<bool>;
}
