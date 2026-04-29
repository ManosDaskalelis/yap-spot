using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence
{
    public sealed class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
    }
}
