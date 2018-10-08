using AutoMapper;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.UserService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.Automapper
{
    public class AutomapperProfile : Profile
    {
        //private readonly IDatabaseService _databaseService;
        //private readonly NotesAppContext _context;

        //public AutomapperProfile(IDatabaseService databaseService, NotesAppContext context)
        //{
        //    _databaseService = databaseService;
        //    _context = context;
        //}

        public AutomapperProfile()
        {
            CreateMap<Users, UserDetail>();
            CreateMap<UserDetail, Users>();

            CreateMap<Users, UserBaseModel>();
            CreateMap<UserBaseModel, Users>();

            CreateMap<Users, UserUpdateModel>();
            CreateMap<UserUpdateModel, Users>();



            CreateMap<Notes, NoteDetail>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(entity => 
                    entity.TagNoteRelationship
                    .Select(relationship => new TagSummary { TagId = relationship.TagId, Title = relationship.Tag.Title})));
            CreateMap<NoteDetail, Notes>();

            CreateMap<NoteUpdateModel, Notes>();
            CreateMap<Notes, NoteUpdateModel>();

            CreateMap<NoteCreationModel, Notes>();
            CreateMap<Notes, NoteCreationModel>();


            CreateMap<Tags, TagSummary>();
            CreateMap<TagSummary, Tags>();

            CreateMap<Tags, TagCreationModel>();
            CreateMap<TagCreationModel, Tags>();

            CreateMap<Tags, TagUpdateModel>();
            CreateMap<TagUpdateModel, Tags>()
                //.ForMember(tags => tags.CreatedByUser, opt => opt.MapFrom(update => 
                    //_databaseService.GetObject<Users, UserBaseModel>(_context.Users, update.CreatedByUserId)))
                ;

            CreateMap<Tags, TagDetail>()
                .ForMember(dest => dest.TagsToAlwaysInclude, opt => opt.MapFrom(t => t.TagNoteRelationship
                    .Select(tr => tr.Tag))
                )
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(t => t.CreatedByUser.Username));
            CreateMap<TagDetail, Tags>();

            CreateMap<TagNoteRelationship, TagSummary>();
            CreateMap<TagSummary, TagNoteRelationship>();
        }
    }
}
