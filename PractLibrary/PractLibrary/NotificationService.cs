using System;
using System.Collections.Generic;
using System.Linq;

namespace PractLibrary
{
    public class NotificationService
    {
        /// <summary>
        /// Сортирует и фильтрует уведомления в списке на основе заданных критериев.
        /// </summary>
        /// <param name="notifications">Исходная коллекция уведомлений.</param>
        /// <param name="options">Параметры фильтрации и сортировки.</param>
        /// <returns>Отфильтрованная и отсортированная последовательность уведомлений.</returns>
        public IEnumerable<Notification> FilterAndSort(
            IEnumerable<Notification> notifications,
            NotificationFilterOptions options)
        {
            // Безопасность: если коллекция не передана, возвращаем пустой результат вместо NullReferenceException
            if (notifications == null)
            {
                return Enumerable.Empty<Notification>();
            }

            // Если параметры фильтрации отсутствуют, возвращаем исходный список в виде ленивой коллекции
            if (options == null)
            {
                return notifications;
            }

            // Преобразуем в IQueryable для оптимального построения цепочки запросов LINQ
            var query = notifications.AsQueryable();

            // ФИЛЬТРАЦИЯ

            // Фильтр по статусу прочтения (проверяем на наличие значения Nullable-типа)
            if (options.IsRead.HasValue)
            {
                query = query.Where(n => n.IsRead == options.IsRead.Value);
            }

            // Фильтр по массиву типов уведомлений
            if (options.Types != null && options.Types.Length > 0)
            {
                query = query.Where(n => options.Types.Contains(n.Type));
            }

            // Текстовый поиск по вхождению в Title или Content (без учета регистра)
            if (!string.IsNullOrWhiteSpace(options.SearchText))
            {
                var search = options.SearchText.Trim();
                query = query.Where(n =>
                    (n.Title != null && n.Title.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Content != null && n.Content.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            // Фильтр по минимально допустимому приоритету
            if (options.MinPriority.HasValue)
            {
                query = query.Where(n => n.Priority >= options.MinPriority.Value);
            }

            // СОРТИРОВКА

            if (options.SortBy.HasValue)
            {
                query = options.SortBy.Value switch
                {
                    SortNotificationBy.Date => options.Descending
                        ? query.OrderByDescending(n => n.CreatedAt)
                        : query.OrderBy(n => n.CreatedAt),

                    SortNotificationBy.Priority => options.Descending
                        ? query.OrderByDescending(n => n.Priority)
                        : query.OrderBy(n => n.Priority),

                    SortNotificationBy.Title => options.Descending
                        ? query.OrderByDescending(n => n.Title)
                        : query.OrderBy(n => n.Title),

                    _ => query
                };
            }

            // Возвращаем результат в виде отложенного перечисления (IEnumerable)
            return query;
        }
    }
}
