using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoApp
{
    public class TodoModel : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _createdAt = DateTime.Now;
        private DateTime _dueDate = DateTime.Now.AddDays(1);
        private TaskPriority _priority = TaskPriority.Medium;
        private TaskStatus _status = TaskStatus.NotStarted;

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название задачи не может быть пустым.");
                _title = value.Trim();
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (value < CreatedAt.Date)
                    throw new ArgumentException("Срок выполнения не может быть раньше даты создания.");
                _dueDate = value;
                OnPropertyChanged();
            }
        }

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        }

        public TaskStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}