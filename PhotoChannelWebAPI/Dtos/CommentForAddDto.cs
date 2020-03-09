using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class CommentForAddDto
    {
        public int PhotoId { get; set; }
        public DateTime ShareDate { get; set; }
        public string Description { get; set; }
    }
}
