using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;

        public string Content { get; set; } = null!;

        public MessageTypeEnum Type { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? EditedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public MessageMetadata? Metadata { get; set; }
        public ICollection<MessageReaction> Reactions { get; set; } = [];
    }
}
