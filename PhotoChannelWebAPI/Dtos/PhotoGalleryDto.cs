using System;

namespace PhotoChannelWebAPI.Dtos
{
    //ToDo:configuration
    public class PhotoGalleryDto
    {
        public string PhotoUrl { get; set; }
        public DateTime ShareDate { get; set; }
        public int LikeCount { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
