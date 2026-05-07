using Chat.Contracts.Rooms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Rooms.Commands.JoinRoom
{
    public sealed record JoinRoomCommand(Guid RoomId): IRequest<JoinRoomResult>;
}
