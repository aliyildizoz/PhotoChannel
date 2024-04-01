using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;
using Entities.Concrete;

namespace Core.Entities.Concrete
{
    public class UserOperationClaim : BaseEntity,IEntity
    {
        public int? UserId { get; set; }
        public int? OperationClaimId { get; set; }

        public virtual User User { get; set; }
        public virtual OperationClaim OperationClaim { get; set; }
    }
}
