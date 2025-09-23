using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Definitions;
using Foundatio.Mediator;

using Microsoft.EntityFrameworkCore;

namespace Foundatio.CommandQuery.EntityFramework;

public abstract class EntityCommandHandler<TContext, TEntity, TKey, TReadModel, TCreateModel, TUpdateModel>
    where TContext : DbContext
    where TEntity : class, IHaveIdentifier<TKey>, new()
{
    private static readonly string _contextName = typeof(TContext).Name;
    private static readonly string _entityName = typeof(TEntity).Name;
    private static readonly string _modelName = typeof(TReadModel).Name;

    protected EntityCommandHandler(TContext dataContext, IMapper mapper)
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

    public virtual async ValueTask<Result<TReadModel>> HandleAsync(
        CreateEntity<TCreateModel, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Model);

        // create new entity from model
        var entity = Mapper.Map<TCreateModel, TEntity>(request.Model);
        if (entity == null)
            return Result<TReadModel>.BadRequest("Could not map create model.");

        // apply create metadata
        if (entity is ITrackCreated createdModel)
        {
            createdModel.Created = request.Activated;
            createdModel.CreatedBy = request.ActivatedBy;
        }

        // apply update metadata
        if (entity is ITrackUpdated updateEntity)
        {
            updateEntity.Updated = request.Activated;
            updateEntity.UpdatedBy = request.ActivatedBy;
        }

        var dbSet = DataContext
            .Set<TEntity>();

        // add to data set, id should be generated
        await dbSet
            .AddAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        // save to database
        await DataContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        var query = DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .TagWith($"CreateEntity; Context:{_contextName}, Entity:{_entityName}, Model:{_modelName}")
            .TagWithCallSite()
            .Where(p => Equals(p.Id, entity.Id));

        var projected = Mapper.ProjectTo<TEntity, TReadModel>(query);

        var result = await projected
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            return Result.NotFound("Could not map read model.");

        return result;
    }

    public virtual async ValueTask<Result<TReadModel>> HandleAsync(
        UpdateEntity<TKey, TUpdateModel, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Id);
        ArgumentNullException.ThrowIfNull(request.Model);

        var dbSet = DataContext
            .Set<TEntity>();

        // don't query if default value
        var entity = !EqualityComparer<TKey>.Default.Equals(request.Id, default)
            ? await dbSet.FindAsync([request.Id], cancellationToken).ConfigureAwait(false)
            : default;

        if (entity == null && !request.Upsert)
            return Result.NotFound($"Entity with id '{request.Id}' not found.");

        // create entity if not found
        if (entity == null)
        {
            entity = new TEntity { Id = request.Id };

            // apply create metadata
            if (entity is ITrackCreated createdModel)
            {
                createdModel.Created = request.Activated;
                createdModel.CreatedBy = request.ActivatedBy;
            }

            await dbSet
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);
        }

        // copy updates from model to entity
        Mapper.Map(request.Model, entity);

        // apply update metadata
        if (entity is ITrackUpdated updateEntity)
        {
            updateEntity.Updated = request.Activated;
            updateEntity.UpdatedBy = request.ActivatedBy;
        }

        // save updates
        await DataContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        var query = DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .TagWith($"UpdateEntity; Context:{_contextName}, Entity:{_entityName}, Model:{_modelName}")
            .TagWithCallSite()
            .Where(p => Equals(p.Id, entity.Id));

        var projected = Mapper.ProjectTo<TEntity, TReadModel>(query);

        var result = await projected
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            return Result.NotFound("Could not map read model.");

        return result;
    }

    public virtual async ValueTask<Result<TReadModel>> HandleAsync(
        DeleteEntity<TKey, TReadModel> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Id);

        var dbSet = DataContext
            .Set<TEntity>();

        var entity = await dbSet
            .FindAsync([request.Id], cancellationToken)
            .ConfigureAwait(false);

        if (entity == null)
            return Result.NotFound($"Entity with id '{request.Id}' not found.");

        // read the entity before deleting it
        var query = DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .TagWith($"DeleteEntity; Context:{_contextName}, Entity:{_entityName}, Model:{_modelName}")
            .TagWithCallSite()
            .Where(p => Equals(p.Id, entity.Id));

        var projected = Mapper.ProjectTo<TEntity, TReadModel>(query);

        var result = await projected
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            return Result<TReadModel>.NotFound("Could not map read model.");

        // apply update metadata
        if (entity is ITrackUpdated updateEntity)
        {
            updateEntity.UpdatedBy = request.ActivatedBy;
            updateEntity.Updated = request.Activated;
        }

        // entity supports soft delete
        if (entity is ITrackDeleted deleteEntity)
        {
            deleteEntity.IsDeleted = true;
        }
        else
        {
            // when history is tracked, need to update the entity with update metadata before deleting
            if (entity is ITrackHistory and ITrackUpdated)
            {
                await DataContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            // delete the entity
            dbSet.Remove(entity);
        }

        await DataContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return result;
    }
}
