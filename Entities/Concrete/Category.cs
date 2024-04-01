using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Category : BaseEntity,IEntity
    {
        public string? Name { get; set; }
    }
}
