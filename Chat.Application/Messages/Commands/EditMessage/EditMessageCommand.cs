using Chat.Contracts.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Messages.Commands.EditMessage
{
    public sealed record EditMessageCommand(Guid MessageId, string Content) : IRequest<MessageEditDto>;
}
