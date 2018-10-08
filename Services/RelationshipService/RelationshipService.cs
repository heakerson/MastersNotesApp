using Microsoft.EntityFrameworkCore;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.RelationshipService
{
    public class RelationshipService : IRelationshipService
    {
        private readonly NotesAppContext _database;
        private readonly IDbHelper _dbHelper;

        public RelationshipService(NotesAppContext database, IDbHelper dbHelper)
        {
            _database = database;
            _dbHelper = dbHelper;
        }


    }
}
