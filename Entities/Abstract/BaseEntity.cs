using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Abstract
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
    }
}
