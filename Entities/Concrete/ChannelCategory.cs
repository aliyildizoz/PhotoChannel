using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class ChannelCategory : BaseEntity,IEntity
    {
        public int? ChannelId { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
