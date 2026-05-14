using Chat.Contracts.Rooms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Commands.LeaveRoom
{
    public sealed record LeaveRoomCommand(Guid RoomId) : IRequest<LeaveRoomResult>;
}
