using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Photo : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public DateTime ShareDate { get; set; }
        public int LikesCount { get; set; }
        public int CommentCount { get; set; }
        public string PhotoUrl { get; set; }
    }
}
