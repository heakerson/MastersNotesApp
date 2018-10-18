using AutoMapper;
using Nerdable.DbHelper.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.NoteService;
using Nerdable.NotesApi.Services.RelationshipService;
using Nerdable.NotesApi.Services.TagService;
using Nerdable.NotesApi.Services.UserService;
using Swashbuckle.AspNetCore.Swagger;
using Nerdable.NotesApi.Services.Automapper;
using System;

namespace Nerdable.NotesApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "EF Core Notes API", Version = "v1" });
            });
            
            //services.AddAutoMapper();

            var connection = "Server=HEATHERA-PC3;Database=NotesApp;Trusted_Connection=True;MultipleActiveResultSets=true";

            services.AddDbContext<NotesAppContext>(options =>
                options.UseSqlServer(connection)
            );

            //services.AddScoped<DbContext, NotesAppContext>();

            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRelationshipService, RelationshipService>();

            services.AddDbHelper<NotesAppContext>();

            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile(provider.GetService<NotesAppContext>()));
            }).CreateMapper());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EF Core API V1");
            });

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
