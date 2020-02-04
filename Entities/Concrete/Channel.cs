using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Channel : IEntity
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int SubscribersCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChannelPhotoUrl { get; set; }
    }
}
