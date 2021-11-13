using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Channel : IEntity
    {
        public Channel()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChannelPhotoUrl { get; set; }
        public string PublicId { get; set; }


        public virtual User User { get; set; }
        public IEnumerable<Subscriber> Subscribers { get; set; }
    }
}
