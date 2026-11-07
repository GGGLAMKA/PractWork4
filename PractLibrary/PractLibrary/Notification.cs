using System;

namespace PractLibrary
{
    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        System
    }

    public enum SortNotificationBy
    {
        Date,
        Priority,
        Title
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Priority { get; set; } // От 0 (низкий) до 5 (высокий)
        public bool IsRead { get; set; }
    }
}
