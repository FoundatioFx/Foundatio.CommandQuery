using System.Linq.Dynamic.Core;

using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.Results;
using Foundatio.Mediator;

using Microsoft.EntityFrameworkCore;

namespace Foundatio.CommandQuery.EntityFramework;

/// <summary>
/// A base handler for a request that requires the specified <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TContext">The type of <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TEntity">The type of entity being operated on by the <see cref="DbContext"/></typeparam>
/// <typeparam name="TKey">The key type for the data context entity</typeparam>
/// <typeparam name="TReadModel">The type of the read model.</typeparam>
public abstract class EntityQueryHandler<TContext, TEntity, TKey, TReadModel>
    where TContext : DbContext
    where TEntity : class, IHaveIdentifier<TKey>, new()
{
    private static readonly string _contextName = typeof(TContext).Name;
    private static readonly string _entityName = typeof(TEntity).Name;
    private static readonly string _modelName = typeof(TReadModel).Name;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityQueryHandler{TContext, TEntity, TKey, TReadModel}"/> class.
    /// </summary>
    /// <inheritdoc/>
    protected EntityQueryHandler(TContext dataContext, IMapper mapper)
    {
        DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    /// <summary>
    /// Gets the <see cref="DbContext"/> for this handler.
    /// </summary>
    protected TContext DataContext { get; }

    /// <summary>
    /// Gets the <see cref="IMapper"/> for this handler.
    /// </summary>
    protected IMapper Mapper { get; }


    public virtual async ValueTask<Result<TReadModel?>> HandleAsync(
        GetEntity<TKey, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Id);

        var query = DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .TagWith($"GetEntity; Context:{_contextName}, Entity:{_entityName}, Model:{_modelName}")
            .TagWithCallSite()
            .Where(p => Equals(p.Id, request.Id));

        var projected = Mapper.ProjectTo<TEntity, TReadModel>(query);

        var result = await projected
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result<TReadModel?>.Success(result);
    }

    public virtual async ValueTask<Result<IReadOnlyList<TReadModel>>> HandleAsync(
        GetEntities<TKey, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Ids);

        var query = DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .TagWith($"GetEntities; Context:{_contextName}, Entity:{_entityName}, Model:{_modelName}")
            .TagWithCallSite()
            .Where(p => request.Ids.Contains(p.Id));

        var projected = Mapper.ProjectTo<TEntity, TReadModel>(query);

        var results = await projected
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result<IReadOnlyList<TReadModel>>.Success(results);
    }

    public virtual async ValueTask<QueryResult<TReadModel>> HandleAsync(
        QueryEntities<TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .TagWith($"EntityPagedQueryHandler; Context:{typeof(TContext).Name}, Entity:{typeof(TEntity).Name}, Model:{typeof(TReadModel).Name}");

        var queryDefinition = request.Query;

        // apply filter before getting entities
        if (queryDefinition?.Filter is not null)
            query = query.Filter(queryDefinition.Filter);

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
                return new QueryResult<TReadModel>([]);

            query = query.Page(queryDefinition.Page.Value, queryDefinition.PageSize.Value);
        }

        // apply sorting and project to read models
        var results = await query
            .Sort(queryDefinition?.Sorts)
            .ProjectTo<TEntity, TReadModel>(Mapper)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new QueryResult<TReadModel>(results) { Total = total };
    }
}
