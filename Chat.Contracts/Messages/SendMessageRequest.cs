using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Messages
{
    public sealed record SendMessageRequest(string content);
}
