using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Persistence.Configurations
{
    public sealed class RoomMemberConfiguration : IEntityTypeConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> builder)
        {
            builder.ToTable("room_members");

            builder.HasKey(x => new { x.RoomId, x.UserId });

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.JoinedAtUtc)
                .IsRequired();

            builder.HasOne(x => x.Room)
                .WithMany(x => x.ConnectedUsers)
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.RoomMemberships)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId);
        }
    }
}
