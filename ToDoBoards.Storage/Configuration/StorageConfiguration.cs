namespace ToDoBoards.Storage.Configuration;

internal class StorageConfiguration
{
    public string ConnectionString { get; set; }
    public InMemoryConfiguration InMemory { get; set; }
}