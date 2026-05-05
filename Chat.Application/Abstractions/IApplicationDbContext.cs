using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Message> Messages { get; }
        public DbSet<Room> Rooms { get; }
        public DbSet<User> Users { get; }
        public DbSet<RoomMember> RoomMembers { get; }
        public DbSet<MessageReaction> MessageReactions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
