using System.Collections.Generic;

namespace TodoApp
{
    public interface ITaskRepository
    {
        void Save(IEnumerable<TodoModel> tasks, string filePath);
        List<TodoModel> Load(string filePath);
    }
}