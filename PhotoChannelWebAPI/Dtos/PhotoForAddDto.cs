using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PhotoChannelWebAPI.Dtos
{
    public class PhotoForAddDto
    {
        public PhotoForAddDto()
        {
            ShareDate = DateTime.Now;
        }
        public int ChannelId { get; set; }
        public IFormFile File { get; set; }
        public DateTime ShareDate { get; set; }
    }
}
