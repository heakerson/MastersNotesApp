using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.DatabaseServices.Models
{
    public class EntityForeignKey<TEntity> : EntityProperty<TEntity>
    {
        //public EntityProperty<TForeignEntity> ForeignKey { get; set; }
        public Type ForeignEntityType { get; }
        public Type ForeignValueType { get; }
        public string ForeignName { get; }
        public object ForeignValue { get; }

        public EntityForeignKey(string name, object value, Type foreignEntityType, string foreignName, object foreignValue) : base(name, value)
        {
            ForeignEntityType = foreignEntityType;
            ForeignName = foreignName;
            ForeignValue = ForeignValue;
            ForeignEntityType = ForeignValue.GetType();
        }
    }
}
