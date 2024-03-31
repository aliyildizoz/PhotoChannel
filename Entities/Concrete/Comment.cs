using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Comment : BaseEntity,IEntity
    {
        public Comment()
        {
            ShareDate = DateTime.Now;
        }
        public int? UserId { get; set; }
        public int? PhotoId { get; set; }
        public DateTime ShareDate { get; set; }
        public string? Description { get; set; }


        public virtual User User { get; set; }
        public virtual Photo Photo { get; set; }
    }
}
