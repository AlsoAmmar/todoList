using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ToDoList.Model;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void DeleteTask(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var item = button!.DataContext as TodoTask;
        ((MainViewModel)DataContext!).DeleteToggleCommand.Execute(item);
    }

    private void EditTask(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var item = button!.DataContext as TodoTask;
        ((MainViewModel)DataContext!).EditToggleCommand.Execute(item);
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        ((MainViewModel)DataContext!).IsVisible = false;
        ((MainViewModel)DataContext!).ClearEverything();
    }
}