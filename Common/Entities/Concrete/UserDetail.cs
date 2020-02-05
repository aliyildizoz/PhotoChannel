using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Entities.Abstract;

namespace Core.Entities.Concrete
{
    public class UserDetail : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubscriptionCount { get; set; }
    }
}
