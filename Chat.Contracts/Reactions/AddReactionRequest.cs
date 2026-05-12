using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Reactions
{
    public sealed record AddReactionRequest(string Emoji);
}
