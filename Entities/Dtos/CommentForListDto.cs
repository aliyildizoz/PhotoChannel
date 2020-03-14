using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Dtos
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
