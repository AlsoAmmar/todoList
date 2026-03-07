using System;
using System.Text.Json.Serialization;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoList.Model;

public partial class TodoTask : ObservableObject
{
    private static int idIncrementor = 1;
    
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _taskName;
    [ObservableProperty] private string _description;
    [ObservableProperty] private bool _isCompleted;
    [ObservableProperty] private string _createdAt;
    [ObservableProperty] private DateTime _createdAtDateTime;

    public TodoTask()
    {
        Id = idIncrementor++;
    }
}