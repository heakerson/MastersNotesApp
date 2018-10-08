using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.DatabaseServices.Models;
using NotesApp.Api.Services.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.DatabaseServices
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DbContext _dbcontext;
        private readonly IMapper _mapper;

        public DatabaseService(DbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public Response<TEntity> AddObject<TCreateModel, TEntity>(TCreateModel createModel) where TEntity : class, new()
        {
            var mappingResponse = MapToNewObject<TCreateModel, TEntity>(createModel);

            if (mappingResponse.Success)
            {
                try
                {
                    var updated = _dbcontext.Add(mappingResponse.Data);
                    var saved = _dbcontext.SaveChanges();

                    return Response<TEntity>.BuildResponse(mappingResponse.Data);
                }
                catch
                {
                    return Response<TEntity>.BuildResponse(null, false, ReturnCode.DatabaseAddFailure, $"Failed to update the database with {createModel}");
                }
            }
            else
            {
                return mappingResponse;
            }
        }

        public Response<TEntity> RemoveEntity<TEntity>(int id) where TEntity : class
        {
            var entityResponse = GetEntity<TEntity>(id);

            if (entityResponse.Success)
            {
                try
                {
                    var removed = _dbcontext.Remove(entityResponse.Data);
                    var saved = _dbcontext.SaveChanges();

                    return Response<TEntity>.BuildResponse(entityResponse.Data);
                }
                catch(Exception e)
                {
                    return Response<TEntity>.BuildResponse(null, false, ReturnCode.DatabaseRemoveFailure, $"Exception occrured when removing {typeof(TEntity)} with id {id} from the database. Exception: {e.Message} {e.StackTrace}");
                }
            }
            else
            {
                return entityResponse;
            }
        }

        public Response<TEntity> UpdateObject<TUpdateModel, TEntity>(TUpdateModel updateModel, int entityId) where TEntity : class, new()
        {
            var entityResponse = GetEntity<TEntity>(entityId);

            if (entityResponse.Success)
            {
                var mappingResult = MapToExistingObject(updateModel, entityResponse.Data);

                if (mappingResult.Success)
                {
                    return SaveObject(mappingResult.Data);
                }
                else
                {
                    return mappingResult;
                }
            }
            else
            {
                return entityResponse;
            }
        }

        public Response<TEntity> UpdateObject<TEntity>(int id, Func<TEntity, Response<TEntity>> updateEntity) where TEntity : class
        {
            var entityResponse = GetEntity<TEntity>(id);

            if (entityResponse.Success)
            {
                return UpdateObject(entityResponse.Data, updateEntity);
            }
            else
            {
                return entityResponse;
            }
        }

        public Response<TEntity> UpdateObject<TEntity>(TEntity entity, Func<TEntity, Response<TEntity>> updateEntity) where TEntity : class
        {
            var updateResponse = updateEntity(entity);

            if (updateResponse.Success)
            {
                return SaveObject(updateResponse.Data);
            }
            else
            {
                return updateResponse;
            }
        }

        public Response<TEntity> UpdateObject<TUpdateType, TEntity>(IQueryable<TEntity> query, TUpdateType updateModel) where TEntity : class, new()
        {
            var entityResponse = GetEntity(query);

            if (entityResponse.Success)
            {
                var mappingResult = MapToExistingObject(updateModel, entityResponse.Data);

                if (mappingResult.Success)
                {
                    return SaveObject(mappingResult.Data);
                }
                else
                {
                    return mappingResult;
                }
            }
            else
            {
                return entityResponse;
            }

        }

        public Response<TEntity> SaveObject<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                var updated = _dbcontext.Update(entity);
                var saved = _dbcontext.SaveChanges();

                return Response<TEntity>.BuildResponse(entity);
            }
            catch (Exception e)
            {
                return Response<TEntity>.BuildResponse(entity, false, ReturnCode.DatabaseUpdateFailure, $"Failed to update the database with {entity}. Exception: {e.Message} {e.StackTrace}");
            }
        }

        public Response<TOutput> GetObject<TEntity, TOutput>(int id) where TEntity : class where TOutput : new()
        {
            var entityResponse = GetEntity<TEntity>(id);

            if (entityResponse.Success)
            {
                var mappingResult = MapToNewObject<TEntity, TOutput>(entityResponse.Data);

                if (mappingResult.Success)
                {
                    return Response<TOutput>.BuildResponse(mappingResult.Data);
                }
                else
                {
                    return mappingResult;
                }
            }
            else
            {
                return Response<TOutput>.BuildResponse(new TOutput(), false, entityResponse.ReturnCode, entityResponse.ReturnMessage);
            }
        }
       
        public Response<TOutput> GetObject<TEntity, TOutput>(IQueryable<TEntity> query) where TOutput : new() where TEntity : new()
        {
            var getEntityResponse = GetEntity(query);

            if (getEntityResponse.Success)
            {
                var mappingResult = MapToNewObject<TEntity, TOutput>(getEntityResponse.Data);

                if (mappingResult.Success)
                {
                    return Response<TOutput>.BuildResponse(mappingResult.Data);
                }
                else
                {
                    return mappingResult;
                }
            }
            else
            {
                return Response<TOutput>.BuildResponse(new TOutput(), false, getEntityResponse.ReturnCode, getEntityResponse.ReturnMessage);
            }
        }

        public Response<Collection<TOutput>> GetObjects<TEntity, TOutput>(DbSet<TEntity> dbSet, IQueryable<TEntity> query) where TOutput : new() where TEntity : class, new()
        {
            var getEntityResponse = GetEntities(query);

            if (getEntityResponse.Success)
            {
                var mappingResult = MapToNewObject<ICollection<TEntity>, Collection<TOutput>>(getEntityResponse.Data);

                if (mappingResult.Success)
                {
                    return Response<Collection<TOutput>>.BuildResponse(mappingResult.Data);
                }
                else
                {
                    return mappingResult;
                }
            }
            else
            {
                return Response<Collection<TOutput>>.BuildResponse(null, false, getEntityResponse.ReturnCode, getEntityResponse.ReturnMessage);
            }
        }

        public Response<TEntity> GetEntity<TEntity>(int id) where TEntity : class
        {
            var dbSetResponse = GetDbSet<TEntity>();

            if (dbSetResponse.Success)
            {
                var entity = dbSetResponse.Data.Find(id);

                if (entity != null)
                {
                    return Response<TEntity>.BuildResponse(entity);
                }
                else
                {
                    return Response<TEntity>.BuildResponse(null, false, ReturnCode.DoesNotExist, $"Entity of type {typeof(TEntity)} with id {id} does not exist");
                }
            }
            else
            {
                return Response<TEntity>.BuildResponse(null, false, dbSetResponse.ReturnCode, dbSetResponse.ReturnMessage);
            }
        }

        public Response<TEntity> GetEntity<TEntity>(IQueryable<TEntity> query) where TEntity : new()
        {
            try
            {
                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    return Response<TEntity>.BuildResponse(entity);
                }
                else
                {
                    return Response<TEntity>.BuildResponse(new TEntity(), false, ReturnCode.Fail, $"Failed getting {typeof(TEntity)} using query {query} from the database");
                }
            }
            catch
            {
                return Response<TEntity>.BuildResponse(new TEntity(), false, ReturnCode.Fail, $"Failed getting {typeof(TEntity)} using query {query} from the database");
            }
        }

        public Response<ICollection<TEntity>> GetEntities<TEntity>(IQueryable<TEntity> query) where TEntity : class, new()
        {
            try
            {
                var collection = query.ToList();

                if (collection.Any())
                {
                    return Response<ICollection<TEntity>>.BuildResponse(query.ToList());
                }
                else
                {
                    return Response<ICollection<TEntity>>.BuildResponse(null, false, ReturnCode.NoEntitiesMatchQuery, $"No entities of type {typeof(TEntity)} matched the query {query}");
                }
            }
            catch
            {
                return Response<ICollection<TEntity>>.BuildResponse(null, false, ReturnCode.Fail, $"Failed retrieving data of type {typeof(TEntity)} from the database");
            }
        }
                
        public Response<TOutput> MapToNewObject<TInput, TOutput>(TInput source) where TOutput : new()
        {
            try
            {
                var mapped = _mapper.Map<TOutput>(source);

                if (mapped != null)
                {
                    return Response<TOutput>.BuildResponse(mapped);
                }
                else
                {
                    return Response<TOutput>.BuildResponse(new TOutput(), false, ReturnCode.MappingFailure, $"Automapper failed to map object of type {typeof(TInput)} to object of type {typeof(TOutput)}");
                }
            }
            catch(Exception e)
            {
                return Response<TOutput>.BuildResponse(new TOutput(), false, ReturnCode.MappingFailure, $"Automapper threw exception when trying to map object of type {typeof(TInput)} to object of type {typeof(TOutput)}. Exception: {e.StackTrace}");
            }
        }

        public Response<TOutput> MapToExistingObject<TInput, TOutput>(TInput source, TOutput destination) where TOutput : new()
        {
            try
            {
                var mapped = _mapper.Map(source, destination);

                if (mapped != null)
                {
                    return Response<TOutput>.BuildResponse(mapped);
                }
                else
                {
                    return Response<TOutput>.BuildResponse(new TOutput(), false, ReturnCode.MappingFailure, $"Automapper failed to map object of type {typeof(TInput)} to object of type {typeof(TOutput)}");
                }
            }
            catch (Exception e)
            {
                return Response<TOutput>.BuildResponse(new TOutput(), false, ReturnCode.MappingFailure, $"Automapper threw exception when trying to map object of type {typeof(TInput)} to object of type {typeof(TOutput)}. Exception: {e.StackTrace}");
            }

        }
        
        public Response<DbSet<TEntity>> GetDbSet<TEntity>() where TEntity : class
        {
            try
            {
                var dbSet = _dbcontext.Set<TEntity>();

                //to catch if exception occurred 
                var local = dbSet.Local;

                return Response<DbSet<TEntity>>.BuildResponse(dbSet);
            }
            catch
            {
                return Response<DbSet<TEntity>>.BuildResponse(null, false, ReturnCode.DbSetDoesNotExist, $"No dbSet of type {typeof(TEntity)} exists in the current dbContext.");
            }


        }

        public Response<IList<string>> GetPrimaryKeyPropNames<IEntityType>()
        {
            var entityType =  _dbcontext.Model.FindEntityType(typeof(IEntityType));

            if (entityType != null)
            {
                var propNames = entityType.FindPrimaryKey().Properties.Select(p => p.Name).ToList();
                return Response<IList<string>>.BuildResponse(propNames);
            }
            else
            {
                return Response<IList<string>>.BuildResponse(null, false, ReturnCode.DbSetDoesNotExist, $"The current dbContext does not contain an entity type of {typeof(IEntityType)}");
            }
        }

        public Response<IList<EntityProperty<TEntity>>> GetPrimaryKeys<TEntity>(TEntity entity)
        {
            var propNameResponse = GetPrimaryKeyPropNames<TEntity>();

            if (propNameResponse.Success)
            {
                IList<EntityProperty<TEntity>> keys = new List<EntityProperty<TEntity>>();

                foreach (string propName in propNameResponse.Data)
                {
                    var value = entity.GetType().GetProperty(propName).GetValue(entity, null);
                    EntityProperty<TEntity> primaryKey = new EntityProperty<TEntity>(propName, value);

                    keys.Add(primaryKey);
                }

                var primaryKeyValue = entity.GetType().GetProperty(propNameResponse.Data[0]).GetValue(entity, null);

                return Response<IList<EntityProperty<TEntity>>>.BuildResponse(keys);
            }
            else
            {
                return Response<IList<EntityProperty<TEntity>>>.BuildResponse(null, false, propNameResponse.ReturnCode, propNameResponse.ReturnMessage);
            }
        }
    }
}
