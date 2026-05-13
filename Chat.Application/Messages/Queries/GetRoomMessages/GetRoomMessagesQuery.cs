using Chat.Contracts.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Messages.Queries.GetRoomMessages
{
    public sealed record GetRoomMessagesQuery(Guid RoomId) : IRequest<IReadOnlyList<MessageDto>>;
}
