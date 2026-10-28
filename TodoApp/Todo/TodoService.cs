using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TodoApp
{
    public class TodoService
    {
        private readonly ITaskRepository _repository;
        public ObservableCollection<TodoModel> Tasks { get; private set; }

        public TodoService(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Tasks = new ObservableCollection<TodoModel>();
        }

        public void AddTask(TodoModel task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            Tasks.Add(task);
        }

        public bool RemoveTask(TodoModel task)
        {
            return Tasks.Remove(task);
        }

        public void UpdateTaskStatus(TodoModel task, TaskStatus newStatus)
        {
            if (task != null)
            {
                task.Status = newStatus;
            }
        }

        public void SaveTasks(string filePath)
        {
            _repository.Save(Tasks, filePath);
        }

        public void LoadTasks(string filePath)
        {
            var loadedTasks = _repository.Load(filePath);
            Tasks.Clear();
            foreach (var task in loadedTasks)
            {
                Tasks.Add(task);
            }
        }
    }
}