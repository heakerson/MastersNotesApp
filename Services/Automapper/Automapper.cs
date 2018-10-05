using AutoMapper;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.DatabaseServices;
using NotesApp.Api.Services.NoteService.Models;
using NotesApp.Api.Services.TagService.Models;
using NotesApp.Api.Services.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.Automapper
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



            CreateMap<Notes, NoteDetail>();
            CreateMap<NoteDetail, Notes>();


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
