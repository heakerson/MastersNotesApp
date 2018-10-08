using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.DatabaseServices.Models
{
    public class EntityProperty<TEntity>
    {
        public readonly Type EntityType = typeof(TEntity);
        public Type ValueType { get; }
        public string Name { get; }
        public object Value { get; }

        public EntityProperty(string name, object value)
        {
            Name = name;
            Value = value;
            ValueType = value.GetType();
        }
    }
}
