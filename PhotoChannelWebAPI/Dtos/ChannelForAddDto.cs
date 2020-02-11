using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PhotoChannelWebAPI.Dtos
{
    public class ChannelForAddDto
    {
        public ChannelForAddDto()
        {
            CreatedDate = DateTime.Now;
        }

        public int OwnerId { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
