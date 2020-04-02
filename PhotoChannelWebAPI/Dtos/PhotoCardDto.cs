using System;
using System.Collections.Generic;
using Entities.Concrete;

namespace PhotoChannelWebAPI.Dtos
{
    public class PhotoCardDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public DateTime ShareDate { get; set; }
        public string PhotoPublicId { get; set; }
        public string UserName { get; set; }
        public string ChannelName { get; set; }
        public string ChannelPublicId { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
    }
}
