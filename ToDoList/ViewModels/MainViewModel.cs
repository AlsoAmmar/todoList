using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Model;

namespace ToDoList.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<TodoTask> _todoTasks = new ObservableCollection<TodoTask>();
    
    [ObservableProperty] [Required(ErrorMessage = "Title is required")] [NotifyDataErrorInfo] [NoEmoji]
    private string _newTitle;
    [ObservableProperty]
    private string _newDescription;
    [ObservableProperty] [Required(ErrorMessage = "Title is required")] [NotifyDataErrorInfo] [NoEmoji]
    private string _editTitle;
    [ObservableProperty]
    private string _editDescription;
    [ObservableProperty] [NotifyDataErrorInfo] [DateOutOfRange]
    private DateTime _editDateTime;
    [ObservableProperty] 
    private DateTime _editDate;
    [ObservableProperty]
    private TimeSpan _editTime;
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsVisible))]
    private bool _isNewVisible;
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsVisible))]
    private bool _isEditVisible;
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsVisible))]
    private bool _isDeleteVisible;
    [ObservableProperty] 
    private bool _isDescriptionVisible;
    [ObservableProperty] [NotifyDataErrorInfo] [DateOutOfRange]
    private DateTime _selectedDateTime;
    [ObservableProperty] 
    private DateTime _selectedDate;
    [ObservableProperty]
    private TimeSpan _selectedTime;

    partial void OnSelectedDateChanged(DateTime value) => UpdateSelectedDateTime();
    partial void OnSelectedTimeChanged(TimeSpan value) => UpdateSelectedDateTime();
    partial void OnEditDateChanged(DateTime value) => UpdateEditDateTime();
    partial void OnEditTimeChanged(TimeSpan value) => UpdateEditDateTime();

    public void UpdateSelectedDateTime()
    {
        SelectedDateTime = SelectedDate + SelectedTime;
    }
    
    public void UpdateEditDateTime()
    {
        EditDateTime = EditDate + EditTime;
    }

    private static TodoTask? _taskToEdit;
    private static TodoTask? _taskToDelete;
    
    public bool IsVisible
    {
        get => IsEditVisible || IsNewVisible || IsDeleteVisible;
        set
        {
            IsEditVisible = value;
            IsNewVisible = value;
            IsDeleteVisible = value;
        }
    }
    

    private static string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static string filePath = Path.Combine(folder, "tasks.json");
    private static string json;

    public MainViewModel()
    {
        if (!File.Exists(filePath))
        {
            var uri = new Uri("avares://ToDoList/Assets/Data/tasks.json");

            using var assetStream = AssetLoader.Open(uri);
            using var fileStream = File.Create(filePath);

            assetStream.CopyTo(fileStream);
        }

        LoadFromJson();
        
        ClearEverything();
        
        SelectedDate = DateTime.Today;
        SelectedTime = DateTime.Now.TimeOfDay;
    }

    [RelayCommand]
    private void NewToggle()
    {
        IsNewVisible = true;
    }

    [RelayCommand]
    private void EditToggle(TodoTask task)
    {
        EditTitle = task.TaskName;
        EditDescription = task.Description;
        EditDateTime = task.CreatedAtDateTime;
        
        IsEditVisible = true;

        _taskToEdit = task;
    }

    [RelayCommand]
    private void DeleteToggle(TodoTask task)
    {
        IsDeleteVisible = true;

        _taskToDelete = task;
        
        if (_taskToDelete.IsCompleted) Delete();
    }

    [RelayCommand]
    private void Save()
    {
        ValidateProperty(NewTitle, nameof(NewTitle));
        if (HasErrors) return;
        
        TodoTasks.Add(new TodoTask()
        {
            TaskName = NewTitle,
            Description = NewDescription,
            CreatedAt = SelectedDate.ToString("ddd, d/M/yyyy h:mm tt"),
            CreatedAtDateTime = SelectedDate,
            IsCompleted = false
        });
        
        SaveToJson();
        IsNewVisible = false;

        ClearEverything();
    }

    [RelayCommand]
    private void Edit()
    {
        ValidateProperty(EditTitle, nameof(EditTitle));
        if (HasErrors) return;

        _taskToEdit!.TaskName = EditTitle;
        _taskToEdit.Description = EditDescription;
        _taskToEdit.CreatedAt = EditDateTime.ToString("ddd, d/M/yyyy h:mm tt");
        _taskToEdit.CreatedAtDateTime = EditDateTime;
        
        SaveToJson();
        IsEditVisible = false;

        ClearEverything();
    }

    [RelayCommand]
    private void Delete()
    {
        TodoTasks.Remove(_taskToDelete!);
        SaveToJson();
        IsDeleteVisible = false;
        
        ClearEverything();
    }

    internal void ClearEverything()
    {
        NewTitle = "";
        NewDescription = "";

        SelectedDate = DateTime.Today;
        SelectedTime = DateTime.Now.TimeOfDay;
        
        EditTitle = "";
        NewDescription = "";
        _taskToEdit = null;
        _taskToDelete = null;
        
        ClearErrors();
    }

    private async void LoadFromJson()
    {
        json = await File.ReadAllTextAsync(filePath);
        TodoTasks = JsonSerializer.Deserialize<ObservableCollection<TodoTask>>(json)!;
    }
    
    private async void SaveToJson()
    {
        json = JsonSerializer.Serialize(TodoTasks, new JsonSerializerOptions{ WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);
    }
}