using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using File = Backend.Models.File;

namespace Backend.Configuration;

public partial class MessengerContext(DbContextOptions<MessengerContext> options) : DbContext(options)
{
    public virtual DbSet<BlockList> BlockLists { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Usr> Usrs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlockList>(entity =>
        {
            entity.HasKey(e => e.BlockListId).HasName("block_list_pkey");

            entity.ToTable("block_list");

            entity.Property(e => e.BlockListId).HasColumnName("block_list_id");
            entity.Property(e => e.BlockUserId).HasColumnName("block_user_id");

            entity.HasOne(d => d.BlockUser).WithMany(p => p.BlockLists)
                .HasForeignKey(d => d.BlockUserId)
                .HasConstraintName("block_list_block_user_id_fkey");
        });

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

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactsId).HasName("contacts_pkey");

            entity.ToTable("contacts");

            entity.Property(e => e.ContactsId).HasColumnName("contacts_id");
            entity.Property(e => e.ContactUserId).HasColumnName("contact_user_id");

            entity.HasOne(d => d.ContactUser).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.ContactUserId)
                .HasConstraintName("contacts_contact_user_id_fkey");
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
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");
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
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("password_hash");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("status");

            entity.HasMany(d => d.BlockListsNavigation).WithMany(p => p.Usrs)
                .UsingEntity<Dictionary<string, object>>(
                    "UsrBlockList",
                    r => r.HasOne<BlockList>().WithMany()
                        .HasForeignKey("BlockListId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usr_block_list_block_list_id_fkey"),
                    l => l.HasOne<Usr>().WithMany()
                        .HasForeignKey("UsrId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usr_block_list_usr_id_fkey"),
                    j =>
                    {
                        j.HasKey("UsrId", "BlockListId").HasName("usr_block_list_pkey");
                        j.ToTable("usr_block_list");
                        j.IndexerProperty<int>("UsrId").HasColumnName("usr_id");
                        j.IndexerProperty<int>("BlockListId").HasColumnName("block_list_id");
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

            entity.HasMany(d => d.ContactsNavigation).WithMany(p => p.Usrs)
                .UsingEntity<Dictionary<string, object>>(
                    "UserContact",
                    r => r.HasOne<Contact>().WithMany()
                        .HasForeignKey("ContactsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_contacts_contacts_id_fkey"),
                    l => l.HasOne<Usr>().WithMany()
                        .HasForeignKey("UsrId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_contacts_usr_id_fkey"),
                    j =>
                    {
                        j.HasKey("UsrId", "ContactsId").HasName("user_contacts_pkey");
                        j.ToTable("user_contacts");
                        j.IndexerProperty<int>("UsrId").HasColumnName("usr_id");
                        j.IndexerProperty<int>("ContactsId").HasColumnName("contacts_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
