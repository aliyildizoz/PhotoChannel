using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class PhotoForListDto
    {
        public string PhotoUrl { get; set; }
        public DateTime ShareDate { get; set; }
        public int LikesCount { get; set; }
        public int CommentCount { get; set; }
    }
}