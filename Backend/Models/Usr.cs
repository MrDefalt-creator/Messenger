using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Usr
{
    public int UsrId { get; set; }

    public string? Login { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Status { get; set; }

    // Чаты, созданные пользователем (init_sender)
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    // Чаты, в которых участвует пользователь (через chat_participant)
    public virtual ICollection<Chat> ChatsNavigation { get; set; } = new List<Chat>();

    // Загруженные файлы
    public virtual ICollection<File> Files { get; set; } = new List<File>();

    // Отправленные сообщения
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    // Refresh токены
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    // Контакты, которых добавил пользователь
    public virtual ICollection<Usr> Contacts { get; set; } = new List<Usr>();

    // Пользователи, у которых текущий пользователь находится в контактах
    public virtual ICollection<Usr> ContactOf { get; set; } = new List<Usr>();

    // Пользователи, которых текущий пользователь заблокировал
    public virtual ICollection<Usr> BlockedUsers { get; set; } = new List<Usr>();

    // Пользователи, которые заблокировали текущего пользователя
    public virtual ICollection<Usr> BlockedBy { get; set; } = new List<Usr>();
}