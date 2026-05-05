using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Persistence.Configurations
{
    public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.EditedAtUtc)
                .IsRequired(false);

            builder.Property(x => x.DeletedAtUtc)
                .IsRequired(false);

            builder.Property(x => x.Metadata)
            .HasColumnType("jsonb")
            .HasConversion(
              metadata => JsonSerializer.Serialize(metadata, null as JsonSerializerOptions),
              json => JsonSerializer.Deserialize<MessageMetadata>(json, null as JsonSerializerOptions)
          )
          .IsRequired(false);

            builder.HasOne(x => x.Room)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Sender)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Reactions)
                .WithOne(x => x.Message)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.RoomId, x.CreatedAtUtc });

            builder.HasIndex(x => x.SenderId);
        }
    }
}
