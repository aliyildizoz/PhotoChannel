using System.Collections.Generic;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class User : BaseEntity,IEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool IsActive { get; set; }

        public string? RefreshToken { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public IEnumerable<Subscriber> Subscribers { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Like> Likes { get; set; }
    }
}
