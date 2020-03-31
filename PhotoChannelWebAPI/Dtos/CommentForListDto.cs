using System;

namespace PhotoChannelWebAPI.Dtos
{
    public class CommentForListDto
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ShareDate { get; set; }
    }
}
