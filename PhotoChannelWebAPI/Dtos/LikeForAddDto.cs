using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class LikeForAddDto
    {
        public int UserId { get; set; }
        public int PhotoId { get; set; }
    }
}
