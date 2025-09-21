using System.Linq.Dynamic.Core;

using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.Queries;
using Foundatio.Mediator;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Foundatio.CommandQuery.MongoDB;

public abstract class EntityQueryHandler<TEntity, TKey, TReadModel>
    : MongoHandle<TEntity, TKey>
    where TEntity : class, IHaveIdentifier<TKey>, new()
{
    protected EntityQueryHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }

    public virtual async ValueTask<Result<TReadModel>> HandleAsync(
        GetEntity<TKey, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Id);

        var result = await Collection
            .Find(p => Equals(p.Id, request.Id))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        // convert entity to read model
        var readModel = Mapper.Map<TEntity, TReadModel>(result);
        if (readModel == null)
            return Result<TReadModel>.NotFound("Could not map read model.");

        return Result<TReadModel>.Success(readModel);

    }

    public virtual async ValueTask<Result<IReadOnlyList<TReadModel>>> HandleAsync(
        GetEntities<TKey, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Ids);

        var keys = new HashSet<TKey>(request.Ids);

        var results = await Collection
            .AsQueryable()
            .Where(p => keys.Contains(p.Id))
            .ProjectTo<TEntity, TReadModel>(Mapper)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result<IReadOnlyList<TReadModel>>.Success(results);
    }

    public virtual async ValueTask<Result<QueryResult<TReadModel>>> HandleAsync(
        QueryEntities<TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = Collection
            .AsQueryable();

        var queryDefinition = request.Query;

        // build query from filter
        query = query.Filter(queryDefinition?.Filter);

        int? total = null;

        // only page if we have a page and page size
        if (queryDefinition?.Page > 0 && queryDefinition?.PageSize > 0)
        {
            // get total for query
            total = await query
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            // short circuit if total is zero
            if (total == 0)
                return Result<QueryResult<TReadModel>>.Success(new());

            query = query.Page(queryDefinition.Page.Value, queryDefinition.PageSize.Value);
        }

        // apply sorting
        query = query
            .Sort(queryDefinition?.Sorts);

        var results = await query
            .ProjectTo<TEntity, TReadModel>(Mapper)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var queryResult = new QueryResult<TReadModel>
        {
            Total = total,
            Data = results
        };

        return Result<QueryResult<TReadModel>>.Success(queryResult);

    }
}
