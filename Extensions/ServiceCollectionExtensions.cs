using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.Automapper;
using Nerdable.NotesApi.Services.DirectoryService;
using Nerdable.NotesApi.Services.NoteService;
using Nerdable.NotesApi.Services.RelationshipService;
using Nerdable.NotesApi.Services.TagService;
using Nerdable.NotesApi.Services.UserService;

namespace Nerdable.NotesApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotesApi(this IServiceCollection services)
        {

            //Updating the automapper to inject the dbContext
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile(provider.GetService<NotesAppContext>()));
            }).CreateMapper());

            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRelationshipService, RelationshipService>();
            services.AddScoped<IDirectoryService, DirectoryService>();

            return services;
        }
    }
}
