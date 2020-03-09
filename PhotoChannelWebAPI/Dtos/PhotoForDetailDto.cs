using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class PhotoForDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public DateTime ShareDate { get; set; }
        public int LikesCount { get; set; }
        public int CommentCount { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
    }
}
