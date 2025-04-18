using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using File = Backend.Models.File;

namespace Backend.Configuration;

public partial class MessengerContext : DbContext
{
    public MessengerContext()
    {
    }

    public MessengerContext(DbContextOptions<MessengerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Usr> Usrs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("chat_pkey");

            entity.ToTable("chat");

            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.ChatType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("chat_type");
            entity.Property(e => e.InitSenderId).HasColumnName("init_sender_id");

            entity.HasOne(d => d.InitSender).WithMany(p => p.Chats)
                .HasForeignKey(d => d.InitSenderId)
                .HasConstraintName("chat_init_sender_id_fkey");

            entity.HasMany(d => d.Files).WithMany(p => p.Chats)
                .UsingEntity<Dictionary<string, object>>(
                    "ChatFile",
                    r => r.HasOne<File>().WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("chat_file_file_id_fkey"),
                    l => l.HasOne<Chat>().WithMany()
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("chat_file_chat_id_fkey"),
                    j =>
                    {
                        j.HasKey("ChatId", "FileId").HasName("chat_file_pkey");
                        j.ToTable("chat_file");
                        j.IndexerProperty<int>("ChatId").HasColumnName("chat_id");
                        j.IndexerProperty<int>("FileId").HasColumnName("file_id");
                    });
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("file_pkey");

            entity.ToTable("file");

            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .HasColumnName("file_name");
            entity.Property(e => e.FileType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("file_type");
            entity.Property(e => e.PathFile).HasColumnName("path_file");
            entity.Property(e => e.UploadedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("uploaded_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Files)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MsgId).HasName("message_pkey");

            entity.ToTable("message");

            entity.Property(e => e.MsgId).HasColumnName("msg_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.MsgText)
                .HasMaxLength(1000)
                .HasColumnName("msg_text");
            entity.Property(e => e.MsgTimestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("msg_timestamp");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("message_chat_id_fkey");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("message_sender_id_fkey");

            entity.HasMany(d => d.Files).WithMany(p => p.Msgs)
                .UsingEntity<Dictionary<string, object>>(
                    "MessageFile",
                    r => r.HasOne<File>().WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("message_file_file_id_fkey"),
                    l => l.HasOne<Message>().WithMany()
                        .HasForeignKey("MsgId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("message_file_msg_id_fkey"),
                    j =>
                    {
                        j.HasKey("MsgId", "FileId").HasName("message_file_pkey");
                        j.ToTable("message_file");
                        j.IndexerProperty<int>("MsgId").HasColumnName("msg_id");
                        j.IndexerProperty<int>("FileId").HasColumnName("file_id");
                    });
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsRevoked)
                .HasDefaultValue(false)
                .HasColumnName("is_revoked");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UsrId).HasColumnName("usr_id");

            entity.HasOne(d => d.Usr).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UsrId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("refresh_tokens_usr_id_fkey");
        });

        modelBuilder.Entity<Usr>(entity =>
        {
            entity.HasKey(e => e.UsrId).HasName("usr_pkey");

            entity.ToTable("usr");

            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("status");
            
                // контакты
                entity.HasMany(d => d.Contacts)
                    .WithMany(p => p.ContactOf)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserContact",
                        j => j.HasOne<Usr>().WithMany().HasForeignKey("ContactId")
                            .HasConstraintName("user_contact_contact_id_fkey"),
                        j => j.HasOne<Usr>().WithMany().HasForeignKey("UserId")
                            .HasConstraintName("user_contact_user_id_fkey"),
                        j =>
                        {
                            j.HasKey("UserId", "ContactId").HasName("user_contact_pkey");
                            j.ToTable("user_contact");
                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                            j.IndexerProperty<int>("ContactId").HasColumnName("contact_id");
                        });

                // Блокировки
                entity.HasMany(d => d.BlockedUsers)
                    .WithMany(p => p.BlockedBy)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserBlock",
                        j => j.HasOne<Usr>().WithMany().HasForeignKey("BlockedUserId")
                            .HasConstraintName("user_block_blocked_user_id_fkey"),
                        j => j.HasOne<Usr>().WithMany().HasForeignKey("UserId")
                            .HasConstraintName("user_block_user_id_fkey"),
                        j =>
                        {
                            j.HasKey("UserId", "BlockedUserId").HasName("user_block_pkey");
                            j.ToTable("user_block");
                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                            j.IndexerProperty<int>("BlockedUserId").HasColumnName("blocked_user_id");
                        });

                entity.HasMany(d => d.ChatsNavigation).WithMany(p => p.Usrs)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserChat",
                        r => r.HasOne<Chat>().WithMany()
                            .HasForeignKey("ChatId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("user_chat_chat_id_fkey"),
                        l => l.HasOne<Usr>().WithMany()
                            .HasForeignKey("UsrId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("user_chat_usr_id_fkey"),
                        j =>
                        {
                            j.HasKey("UsrId", "ChatId").HasName("user_chat_pkey");
                            j.ToTable("user_chat");
                            j.IndexerProperty<int>("UsrId").HasColumnName("usr_id");
                            j.IndexerProperty<int>("ChatId").HasColumnName("chat_id");
                        });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
