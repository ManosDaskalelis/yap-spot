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
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AuthUserId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Username)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DisplayName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.AvatarUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ActivityStatus)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.HasIndex(x => x.AuthUserId)
                .IsUnique();

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.HasMany(x => x.RoomMemberships)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
