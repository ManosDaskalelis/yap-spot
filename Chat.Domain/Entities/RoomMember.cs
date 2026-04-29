using Chat.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entities
{
    public class RoomMember
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public RoomMemberRoleEnum Role { get; set; }
        public DateTime JoinedAtUtc { get; set; }
    }
}
