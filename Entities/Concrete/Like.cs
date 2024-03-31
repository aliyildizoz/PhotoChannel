using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
   public class Like : BaseEntity,IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhotoId { get; set; }

        public virtual User User { get; set; }
        public virtual Photo Photo { get; set; }
    }
}
