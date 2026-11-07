namespace PractLibrary
{
    public class NotificationFilterOptions
    {
        public bool? IsRead { get; set; }
        public NotificationType[]? Types { get; set; } // Список нужных типов
        public string? SearchText { get; set; }
        public int? MinPriority { get; set; }
        public SortNotificationBy? SortBy { get; set; } // По Date, Priority, Title
        public bool Descending { get; set; }
    }
}
