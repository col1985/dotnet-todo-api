using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using TodoApi.Models;
using TodoApi.Repositories; // Add this line

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "TodoItems";

        public TodoRepository(IMemoryCache cache)
        {
            _cache = cache;
            if (!_cache.TryGetValue(CacheKey, out List<TodoItem> items))
            {
                items = new List<TodoItem>();
                _cache.Set(CacheKey, items);
            }
        }

        public IEnumerable<TodoItem> GetAll()
        {
            _cache.TryGetValue(CacheKey, out List<TodoItem> items);
            return items;
        }

        public TodoItem GetById(Guid id)
        {
            _cache.TryGetValue(CacheKey, out List<TodoItem> items);
            return items?.FirstOrDefault(x => x.Id == id);
        }

        public void Add(TodoItem item)
        {
            _cache.TryGetValue(CacheKey, out List<TodoItem> items);
            item.Id = Guid.NewGuid();
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            items.Add(item);
            _cache.Set(CacheKey, items);
        }

        public void Update(TodoItem item)
        {
            _cache.TryGetValue(CacheKey, out List<TodoItem> items);
            var existingItem = items.FirstOrDefault(x => x.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.IsComplete = item.IsComplete;
                existingItem.UpdatedAt = DateTime.UtcNow;
                _cache.Set(CacheKey, items);
            }
        }
    }
}