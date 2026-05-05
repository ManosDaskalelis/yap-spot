using Chat.Domain.Enums;

namespace Chat.Domain.Entities
{
    public class RoomMember
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public RoomMemberRoleEnum Role { get; set; }
        public DateTime JoinedAtUtc { get; set; }
    }
}
