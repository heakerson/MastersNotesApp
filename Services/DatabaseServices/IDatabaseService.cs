using Microsoft.EntityFrameworkCore;
using NotesApp.Api.Services.DatabaseServices.Models;
using NotesApp.Api.Services.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.DatabaseServices
{
    public interface IDatabaseService
    {
        Response<TEntity> AddObject<TCreateModel, TEntity>(TCreateModel createModel) where TEntity : class, new();


        Response<TEntity> UpdateObject<TUpdateModel, TEntity>(TUpdateModel updateModel, int entityId) where TEntity : class, new();
        Response<TEntity> UpdateObject<TEntity>(int id, Func<TEntity, Response<TEntity>> updateEntity) where TEntity : class;
        Response<TEntity> UpdateObject<TEntity>(TEntity entity, Func<TEntity, Response<TEntity>> updateEntity) where TEntity : class;
        Response<TEntity> UpdateObject<TUpdateType, TEntity>(IQueryable<TEntity> query, TUpdateType updateModel) where TEntity : class, new();

        Response<TEntity> SaveObject<TEntity>(TEntity entity) where TEntity : class;

        Response<TOutput> GetObject<TEntity, TOutput>(int id) where TEntity : class where TOutput : new();
        Response<TOutput> GetObject<TEntity, TOutput>(IQueryable<TEntity> query) where TOutput : new() where TEntity : new();
        Response<Collection<TOutput>> GetObjects<TEntity, TOutput>(DbSet<TEntity> dbSet, IQueryable<TEntity> query) where TOutput : new() where TEntity : class, new();


        Response<TEntity> GetEntity<TEntity>(int id) where TEntity : class;
        Response<TEntity> GetEntity<TEntity>(IQueryable<TEntity> query) where TEntity : new();
        Response<ICollection<TEntity>> GetEntities<TEntity>(IQueryable<TEntity> query) where TEntity : class, new();


        Response<TEntity> RemoveEntity<TEntity>(int id) where TEntity : class;


        Response<TOutput> MapToNewObject<TInput, TOutput>(TInput toMap) where TOutput : new();
        Response<TOutput> MapToExistingObject<TInput, TOutput>(TInput source, TOutput destination) where TOutput : new();


        Response<DbSet<TEntity>> GetDbSet<TEntity>() where TEntity : class;


        Response<IList<string>> GetPrimaryKeyPropNames<TEntity>();
        //Response<IList<string>> GetForeignKeyPropNames<TEntity>();

        Response<IList<EntityProperty<TEntity>>> GetPrimaryKeys<TEntity>(TEntity entity);
        //Response<IList<EntityForeignKey<TEntity>>> GetForeignKeys<TEntity>(TEntity entity);
    }
}
