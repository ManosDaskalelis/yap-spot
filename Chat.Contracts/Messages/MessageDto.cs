using Chat.Contracts.Reactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Messages
{
    public sealed record MessageDto(Guid Id, Guid RoomId, Guid SenderId, string Content, string Type, DateTime CreatedAtUtc, IReadOnlyList<MessageReactionSummaryDto> Reactions);
}
