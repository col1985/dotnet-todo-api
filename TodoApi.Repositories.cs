using TodoApi.Models;

namespace TodoApi.Repositories
{
  public interface ITodoRepository
  {
    IEnumerable<TodoItem> GetAll();
    TodoItem GetById(Guid id);
    void Add(TodoItem item);
    void Update(TodoItem item);
  }
}