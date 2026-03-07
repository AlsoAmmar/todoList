using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ToDoList.Model;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ObservableCollection<TodoTask>))]
[JsonSerializable(typeof(TodoTask))]
public partial class JsonAppContext : JsonSerializerContext
{
}