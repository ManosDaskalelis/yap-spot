using Chat.Application.Abstractions;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence
{
    public sealed class ChatDbContext : DbContext, IApplicationDbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomMember> RoomMembers => Set<RoomMember>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<MessageReaction> MessageReactions => Set<MessageReaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);
        }
    }
}
