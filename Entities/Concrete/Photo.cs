using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Photo : BaseEntity,IEntity
    {
        public Photo()
        {
            ShareDate = DateTime.Now;
        }
        public int? UserId { get; set; }
        public int? ChannelId { get; set; }
        public DateTime ShareDate { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PublicId { get; set; }

        public virtual User User { get; set; }
        public virtual Channel Channel { get; set; }

        public virtual List<Like> Likes { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
