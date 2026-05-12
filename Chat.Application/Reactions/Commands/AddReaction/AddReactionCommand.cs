using Chat.Contracts.Reactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Reactions.Commands.AddReaction
{
    public sealed record AddReactionCommand(Guid MessageId, string Emoji): IRequest<ReactionDto>;
}
