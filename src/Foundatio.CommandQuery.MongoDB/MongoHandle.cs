using Foundatio.CommandQuery.Definitions;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB;

public abstract class MongoHandle<TEntity, TKey>
    where TEntity : class, IHaveIdentifier<TKey>, new()
{
    private readonly Lazy<IMongoCollection<TEntity>> _collection;

    protected MongoHandle(IMongoDatabase database, IMapper mapper)
    {
        Database = database;
        Mapper = mapper;

        _collection = new Lazy<IMongoCollection<TEntity>>(CreateCollection);
    }


    protected IMongoCollection<TEntity> Collection => _collection.Value;

    /// <summary>
    /// Gets the <see cref="IMongoDatabase"/> for this handler.
    /// </summary>
    protected IMongoDatabase Database { get; }

    /// <summary>
    /// Gets the <see cref="IMapper"/> for this handler.
    /// </summary>
    protected IMapper Mapper { get; }

    protected virtual string CollectionName()
    {
        return typeof(TEntity).Name;
    }

    protected virtual IMongoCollection<TEntity> CreateCollection()
    {
        var database = Database;

        string collectionName = CollectionName();
        return database.GetCollection<TEntity>(collectionName);
    }
}
