using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class PhotoCardDto
    {
        public Photo Photo { get; set; }
        public string UserName { get; set; }
        public string ChannelName { get; set; }
        public List<CommentForListDto> Comments { get; set; }
        public List<LikeForUserListDto> Likes { get; set; }
    }
}
