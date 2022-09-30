using Xunit;

namespace ToDoBoards.Storage.Test;

[CollectionDefinition(CollectionName)]
public class DbTestCollection : ICollectionFixture<DbFixture>
{
    public const string CollectionName = "DB test collection";
}