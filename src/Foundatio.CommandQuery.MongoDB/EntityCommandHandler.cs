using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Definitions;
using Foundatio.Mediator;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB;

public abstract class EntityCommandHandler<TEntity, TKey, TReadModel, TCreateModel, TUpdateModel>
    : MongoHandle<TEntity, TKey>
    where TEntity : class, IHaveIdentifier<TKey>, new()
{
    protected EntityCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }

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

        await Collection
            .InsertOneAsync(entity, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        // convert to read model
        var readModel = Mapper.Map<TEntity, TReadModel>(entity);
        if (readModel == null)
            return Result<TReadModel>.NotFound("Could not map read model.");

        return Result<TReadModel>.Success(readModel);
    }

    public virtual async ValueTask<Result<TReadModel>> HandleAsync(
       UpdateEntity<TKey, TUpdateModel, TReadModel> request,
       CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Id);
        ArgumentNullException.ThrowIfNull(request.Model);

        var entity = await Collection
            .Find(p => Equals(p.Id, request.Id))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (entity == null && !request.Upsert)
            return Result<TReadModel>.NotFound($"Entity with id '{request.Id}' not found.");

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
        var options = new ReplaceOptions { IsUpsert = request.Upsert };

        await Collection
            .ReplaceOneAsync(p => Equals(p.Id, request.Id), entity, options, cancellationToken)
            .ConfigureAwait(false);

        // return read model
        var result = Mapper.Map<TEntity, TReadModel>(entity);
        if (result == null)
            return Result<TReadModel>.NotFound("Could not map read model.");

        return Result<TReadModel>.Success(result);
    }


    public virtual async ValueTask<Result<TReadModel>> HandleAsync(
      DeleteEntity<TKey, TReadModel> request,
      CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Id);

        var entity = await Collection
            .Find(p => Equals(p.Id, request.Id))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (entity == null)
            return Result<TReadModel>.NotFound($"Entity with id '{request.Id}' not found.");

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
                var replaceResult = await Collection
                    .ReplaceOneAsync(p => Equals(p.Id, request.Id), entity, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            // delete the entity
            var deleteResult = await Collection
                .DeleteOneAsync(p => Equals(p.Id, request.Id), cancellationToken)
                .ConfigureAwait(false);
        }

        // return read model
        var result = Mapper.Map<TEntity, TReadModel>(entity);
        if (result == null)
            return Result<TReadModel>.NotFound("Could not map read model.");

        return Result<TReadModel>.Success(result);
    }
}
