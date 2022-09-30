namespace ToDoBoards.Storage.Configuration;

internal class DbStorage
{
    public string ConnectionString { get; set; }
    public DbStorageInMemory InMemoryDb { get; set; }
}