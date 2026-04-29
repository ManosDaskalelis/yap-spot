using Chat.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public String RoomName { get; set; }
        public RoomTypeEnum Type { get; set; }

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }

        public ICollection<RoomMember> ConnectedUsers { get; set; }
        public ICollection<Message> Messages { get; set; } = [];
    }
}
