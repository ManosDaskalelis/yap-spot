using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.ValueObjects
{
    public class MessageMetadata
    {
        public Guid? ReplyToMessageId { get; set; }
        public bool IsPinned { get; set; }
        public string? AttachmentUrl { get; set; }
        public List<string> Mentions { get; set; }
    }
}
