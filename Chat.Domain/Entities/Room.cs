using Chat.Domain.Enums;

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
