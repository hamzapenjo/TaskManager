using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Task_Manager.DataService;
using Task_Manager.Models;

namespace Task_Manager.ViewModels
{
    public class NewTaskViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public TaskImportance TaskImportance { get; set; }
        public TaskCategory TaskCategory { get; set; } = TaskCategory.Education;
        public ObservableCollection<TaskCheckList> TaskCheckList { get; set; } = new ObservableCollection<TaskCheckList>();
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        public ICommand SaveTaskCommand { get; }
        public ICommand AddSubTaskCommand { get; }

        private readonly TaskDataService _taskDataService;
        private readonly Task _editingTask;

        private string _currentSubTaskDescription;
        public string CurrentSubTaskDescription
        {
            get => _currentSubTaskDescription;
            set
            {
                _currentSubTaskDescription = value;
                OnPropertyChanged(nameof(CurrentSubTaskDescription));
            }
        }

        private int _selectedHour;
        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                OnPropertyChanged(nameof(SelectedHour));
            }
        }

        private int _selectedMinute;
        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                _selectedMinute = value;
                OnPropertyChanged(nameof(SelectedMinute));
            }
        }

        public string SaveButtonText => _editingTask != null ? "Edit Task" : "Add Task";

        public NewTaskViewModel(TaskDataService taskDataService)
        {
            _taskDataService = taskDataService;
            SaveTaskCommand = new RelayCommand(SaveTask);
            AddSubTaskCommand = new RelayCommand(AddSubTask);
        }

        public NewTaskViewModel(Task taskToEdit, TaskDataService taskDataService)
            : this(taskDataService)
        {
            _editingTask = taskToEdit;
            Title = taskToEdit.Title;
            Description = taskToEdit.Description;
            DueDate = taskToEdit.DueDate;
            TaskImportance = taskToEdit.TaskImportance;
            TaskCategory = taskToEdit.TaskCategory;

            TaskCheckList = new ObservableCollection<TaskCheckList>(taskToEdit.TaskCheckList ?? new ObservableCollection<TaskCheckList>());
        }

        private void AddSubTask()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSubTaskDescription))
            {
                TaskCheckList.Add(new TaskCheckList { Description = CurrentSubTaskDescription });
                CurrentSubTaskDescription = string.Empty;
                OnPropertyChanged(nameof(TaskCheckList));
            }
        }

        private void SaveTask()
        {
            DueDate = new DateTime(DueDate.Year, DueDate.Month, DueDate.Day, SelectedHour, SelectedMinute, 0);

            if (_editingTask != null)
            {
                _editingTask.Title = Title;
                _editingTask.Description = Description;
                _editingTask.DueDate = DueDate;
                _editingTask.TaskImportance = TaskImportance;
                _editingTask.TaskCategory = TaskCategory;
                _editingTask.TaskCheckList = new ObservableCollection<TaskCheckList>(TaskCheckList);

                _taskDataService.UpdateTask(_editingTask);
            }
            else
            {
                var newTask = new Task
                {
                    Title = Title,
                    Description = Description,
                    DueDate = DueDate,
                    TaskImportance = TaskImportance,
                    TaskCategory = TaskCategory,
                    TaskCheckList = new ObservableCollection<TaskCheckList>(TaskCheckList),
                    StartDate = DateTime.Now,
                    isComplete = false,
                    TaskState = TaskState.Late,
                    Timer = TimeSpan.Zero,
                    Id = _taskDataService.GenerateNewTaskId()
                };

                _taskDataService.AddTask(newTask);
            }

            var mainWindowViewModel = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault()?.DataContext as MainWindowViewModel;

            mainWindowViewModel?.RefreshTasks();

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w.IsActive)?.Close();
        }


    }
}
