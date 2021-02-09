using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class SearchTextDto
    {
        public string Text { get; set; }
        public List<UserForListDto> Users { get; set; }
        public List<ChannelForListDto> Channels { get; set; }
    }
}
