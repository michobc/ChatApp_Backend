using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace lab9_hub.Models;

public partial class ChatContext : DbContext
{
    public ChatContext()
    {
    }

    public ChatContext(DbContextOptions<ChatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=chatapp;Username=postgres;Password=mypass03923367");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_rooms_pkey");

            entity.ToTable("chat_rooms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasMany(d => d.Messages).WithMany(p => p.ChatRooms)
                .UsingEntity<Dictionary<string, object>>(
                    "ChatRoomMessage",
                    r => r.HasOne<Message>().WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("chat_room_messages_message_id_fkey"),
                    l => l.HasOne<ChatRoom>().WithMany()
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("chat_room_messages_chat_room_id_fkey"),
                    j =>
                    {
                        j.HasKey("ChatRoomId", "MessageId").HasName("chat_room_messages_pkey");
                        j.ToTable("chat_room_messages");
                        j.IndexerProperty<int>("ChatRoomId").HasColumnName("chat_room_id");
                        j.IndexerProperty<int>("MessageId").HasColumnName("message_id");
                    });
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("messages_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReceiverUsername)
                .HasMaxLength(100)
                .HasColumnName("receiver_username");
            entity.Property(e => e.SenderUsername)
                .HasMaxLength(100)
                .HasColumnName("sender_username");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");

            entity.HasOne(d => d.ReceiverUsernameNavigation).WithMany(p => p.MessageReceiverUsernameNavigations)
                .HasForeignKey(d => d.ReceiverUsername)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("messages_receiver_username_fkey");

            entity.HasOne(d => d.SenderUsernameNavigation).WithMany(p => p.MessageSenderUsernameNavigations)
                .HasForeignKey(d => d.SenderUsername)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("messages_sender_username_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
