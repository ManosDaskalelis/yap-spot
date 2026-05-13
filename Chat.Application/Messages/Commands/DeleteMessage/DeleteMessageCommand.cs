using Chat.Contracts.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Messages.Commands.DeleteMessage
{
    public sealed record DeleteMessageCommand(Guid MessageId) : IRequest<MessageDeleteDto>;
}
