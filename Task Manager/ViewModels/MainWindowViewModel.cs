using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Task_Manager.DataService;
using Task_Manager.Models;
using Task_Manager.Views;

namespace Task_Manager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        private ObservableCollection<Task> _filteredTasks;
        public ObservableCollection<Task> FilteredTasks
        {
            get => _filteredTasks;
            set
            {
                _filteredTasks = value;
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private readonly TaskDataService _taskDataService;

        public ICommand IOpenNewWindow { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand CompleteTaskCommand { get; }
        public MainWindowViewModel()
        {
            _taskDataService = new TaskDataService();
            LoadTasks();

            IOpenNewWindow = new RelayCommand(OpenNewTaskWindow);
            DeleteTaskCommand = new RelayCommand(DeleteSelectedTask, CanDeleteTask);
            EditTaskCommand = new RelayCommand(EditSelectedTask, CanEditTask);
            SearchCommand = new RelayCommand(ApplySearch);
            CompleteTaskCommand = new RelayCommand(MarkTaskAsCompleted, CanCompleteTask);


        }

        private bool CanCompleteTask()
        {
            return SelectedTask != null && !SelectedTask.isComplete;
        }
        private void MarkTaskAsCompleted()
        {
            if (SelectedTask != null)
            {
                SelectedTask.isComplete = true;

                _taskDataService.UpdateTask(SelectedTask);

                ApplySearch();
                OnPropertyChanged(nameof(Task));
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private void DeleteSelectedTask()
        {
            try
            {
                if (SelectedTask != null)
                {
                    Tasks.Remove(SelectedTask);
                    _taskDataService.DeleteTasks(SelectedTask.Id);
                    SelectedTask = null;
                    ApplySearch();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
            }
        }

        private bool CanDeleteTask()
        {
            return SelectedTask != null && Tasks.Contains(SelectedTask);
        }


        private void LoadTasks()
        {
            var taskList = _taskDataService.LoadTasks();
            Tasks.Clear();
            foreach (var task in taskList)
            {
                Tasks.Add(task);
            }
            ApplySearch();
        }

        private void OpenNewTaskWindow()
        {
            var newTaskWindow = new NewTaskWindow
            {
                DataContext = new NewTaskViewModel(_taskDataService)
            };
            newTaskWindow.ShowDialog();
            LoadTasks();
        }

        private bool CanEditTask()
        {
            return SelectedTask != null;
        }

        private void EditSelectedTask()
        {
            if (SelectedTask == null) return;

            var editWindow = new NewTaskWindow
            {
                DataContext = new NewTaskViewModel(SelectedTask, _taskDataService)
            };

            editWindow.ShowDialog();
            LoadTasks();
        }

        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredTasks = new ObservableCollection<Task>(Tasks);
            }
            else
            {
                FilteredTasks = new ObservableCollection<Task>(
                    Tasks.Where(t =>
                        (t.Title?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (t.Description?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)));
            }
        }
        public void RefreshTasks()
        {
            LoadTasks();
        }
    }
}
