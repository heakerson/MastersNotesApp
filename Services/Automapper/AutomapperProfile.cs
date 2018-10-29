using AutoMapper;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.UserService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Nerdable.NotesApi.Services.DirectoryService.Models;

namespace Nerdable.NotesApi.Services.Automapper
{
    public class AutomapperProfile : Profile
    {
        public NotesAppContext _dbContext { get; set; }

        public AutomapperProfile(){ }

        public AutomapperProfile(NotesAppContext context)
        {
            _dbContext = context;

            CreateMap<Users, UserDetail>()
                .ForMember(dest => dest.NotesCreated, opt => opt.MapFrom(users => users.Notes))
                .ForMember(dest => dest.TagsCreated, opt => opt.MapFrom(users => users.Tags));
            CreateMap<UserDetail, Users>();

            CreateMap<Users, UserBaseModel>();
            CreateMap<UserBaseModel, Users>();

            CreateMap<Users, UserUpdateModel>();
            CreateMap<UserUpdateModel, Users>();

            CreateMap<Notes, NoteDetail>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(entity => 
                    entity.TagNoteRelationship
                    .Where(r => string.IsNullOrEmpty(r.Tag.ParentTitle))
                    .Select(relationship => new TagSummary { TagId = relationship.TagId, Title = relationship.Tag.Title})))
                .ForMember(dest => dest.Directories, opt => opt.MapFrom(entity =>
                    entity.TagNoteRelationship
                    .Where(r => !string.IsNullOrEmpty(r.Tag.ParentTitle))
                    .Select(relationship => new TagSummary { TagId = relationship.TagId, Title = relationship.Tag.Title })));
            CreateMap<NoteDetail, Notes>();

            CreateMap<NoteUpdateModel, Notes>();
            CreateMap<Notes, NoteUpdateModel>();
                //.ForMember(dest => dest.Tags, opt => opt.MapFrom(entity => entity.TagNoteRelationship.Select(relationship => new TagSummary { TagId = relationship.TagId, Title = relationship.Tag.Title })));

            CreateMap<NoteCreationModel, Notes>();
            CreateMap<Notes, NoteCreationModel>();

            CreateMap<Notes, NoteBase>();
            CreateMap<NoteBase, Notes>();

            CreateMap<Notes, NoteSummary>();
            CreateMap<NoteSummary, Notes>();


            CreateMap<Tags, Tags>();

            CreateMap<Tags, TagSummary>();
            CreateMap<TagSummary, Tags>();

            CreateMap<Tags, DirectorySummary>();
            CreateMap<DirectorySummary, Tags>();

            CreateMap<Tags, TagCreationModel>();
            CreateMap<TagCreationModel, Tags>();

            CreateMap<Tags, TagUpdateModel>();
            CreateMap<TagUpdateModel, Tags>();

            CreateMap<Tags, TagDetail>()
                //.ForMember(dest => dest.ParentTags, opt => opt.MapFrom(t => _dbContext.TagAlwaysIncludeRelationship
                //    .Where(tr => tr.ChildTagId == t.TagId)
                //    .Select(tr => tr.AlwaysIncludeTag)
                //    ))
                //.ForMember(dest => dest.ChildTags, opt => opt.MapFrom(t => _dbContext.TagAlwaysIncludeRelationship
                //    .Where(tr => tr.AlwaysIncludeTagId == t.TagId)
                //    .Select(rel => rel.ChildTag)))
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(t => t.CreatedByUser.Username));
            CreateMap<TagDetail, Tags>();

            CreateMap<Tags, DirectoryDetail>()
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(t => t.CreatedByUser.Username))
                .ForMember(dest => dest.ParentIds, opt => opt.MapFrom(t => MapPathIds(t.PathWithIds)))
                ;
            CreateMap<DirectoryDetail, Tags>();


            CreateMap<TagNoteRelationship, TagSummary>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(rel => rel.Tag.Title));
            CreateMap<TagSummary, TagNoteRelationship>()
                .ForMember(tr => tr.TagId, opt => opt.MapFrom(ts => ts.TagId))
                .ForMember(tr => tr.Tag, opt => opt.MapFrom(ts => _dbContext.Tags.Find(ts.TagId)));

            CreateMap<TagNoteRelationship, NoteDetail>()
                //.ForMember(destinationMember => destinationMember, opt => opt.MapFrom(tn => tn.Note))
                .ForAllMembers(opt => opt.MapFrom(tn => tn.Note))
                ;
            CreateMap<NoteDetail, TagNoteRelationship>();
        }


        public List<int> MapPathIds(string path)
        {
            List<int> Ids = new List<int>();
            var stringList = path.Split("/", StringSplitOptions.RemoveEmptyEntries);

            foreach(string idString in stringList)
            {
                int id = int.Parse(idString);

                Ids.Add(id);
            }

            return Ids;
        }
    }
}
