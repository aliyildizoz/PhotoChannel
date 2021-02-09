using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;

namespace PhotoChannelWebAPI.Dtos
{
    public class SearchByCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChannelPhotoUrl { get; set; }
        public string PublicId { get; set; }
        public int SubscribersCount { get; set; }
        public int OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
