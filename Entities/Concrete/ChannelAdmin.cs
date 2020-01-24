using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class ChannelAdmin : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
    }
}
