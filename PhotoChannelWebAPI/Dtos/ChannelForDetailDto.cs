using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class ChannelForDetailDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int SubscribersCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChannelPhotoUrl { get; set; }
        public string PublicId { get; set; }
    }
}
