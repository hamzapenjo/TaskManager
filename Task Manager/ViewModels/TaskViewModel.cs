using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Task_Manager.DataService;
using Task_Manager.Models;

namespace Task_Manager.ViewModels
{
    internal class TaskViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool isComplete { get; set; }
        public TimeSpan Timer { get; set; }
        public TaskState TaskState { get; set; }
        public TaskImportance TaskImportance { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public ObservableCollection<TaskCheckList> TaskCheckList { get; set; }

        public ICommand IAddNewTask => new RelayCommand(AddNewTask);
        public ICommand AddSubTaskCommand { get; private set; }

        private readonly TaskDataService _taskDataService;
        private ObservableCollection<Task> _tasks;
        private int _selectedHour;
        private int _selectedMinute;
        private string _currentSubTaskDescription;

        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                if (_selectedHour != value)
                {
                    _selectedHour = value;
                    OnPropertyChanged(nameof(SelectedHour));
                }
            }
        }

        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                if (_selectedMinute != value)
                {
                    _selectedMinute = value;
                    OnPropertyChanged(nameof(SelectedMinute));
                }
            }
        }

        public string CurrentSubTaskDescription
        {
            get => _currentSubTaskDescription;
            set
            {
                if (_currentSubTaskDescription != value)
                {
                    _currentSubTaskDescription = value;
                    OnPropertyChanged(nameof(CurrentSubTaskDescription));
                }
            }
        }

        public TaskViewModel()
        {
            _taskDataService = new TaskDataService();
            TaskCheckList = new ObservableCollection<TaskCheckList>();
            DueDate = DateTime.Now;
            AddSubTaskCommand = new RelayCommand(AddSubTask);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadTasks()
        {
            var TaskList = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(TaskList);
        }

        private void AddSubTask()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSubTaskDescription))
            {
                TaskCheckList.Add(new TaskCheckList { Description = CurrentSubTaskDescription });
                CurrentSubTaskDescription = "";
                OnPropertyChanged(nameof(CurrentSubTaskDescription));
            }
        }

        public void AddNewTask()
        {
            DueDate = new DateTime(DueDate.Year, DueDate.Month, DueDate.Day, SelectedHour, SelectedMinute, 0);

            Task newTask = new Task
            {
                Title = this.Title,
                Description = this.Description,
                Id = _taskDataService.GenerateNewTaskId(),
                DueDate = this.DueDate,
                isComplete = false,
                StartDate = DateTime.Now,
                TaskCategory = TaskCategory.Education,
                TaskCheckList = this.TaskCheckList,
                TaskImportance = this.TaskImportance,
                TaskState = TaskState.Late,
                Timer = new TimeSpan(0)
            };

            _taskDataService.AddTask(newTask);
            ClearFields();

            LoadTasks();
        }

        private void ClearFields()
        {
            Title = "";
            Description = "";
            DueDate = DateTime.Now;
            TaskCheckList.Clear();
            SelectedHour = 0;
            SelectedMinute = 0;
            CurrentSubTaskDescription = "";

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(DueDate));
            OnPropertyChanged(nameof(TaskCheckList));
            OnPropertyChanged(nameof(SelectedHour));
            OnPropertyChanged(nameof(SelectedMinute));
            OnPropertyChanged(nameof(CurrentSubTaskDescription));
        }

        public void UpdateTask(Task updateTask)
        {
            _taskDataService.UpdateTask(updateTask);
            LoadTasks();
        }

        public void DeleteTask(int taskId)
        {
            _taskDataService.DeleteTasks(taskId);
            LoadTasks();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
