using Chat.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string AuthUserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
        public UserStatusEnum ActivityStatus { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public ICollection<RoomMember> RoomMemberships { get; set; } = [];
        public ICollection<Message> Messages { get; set; } = [];
    }
}
