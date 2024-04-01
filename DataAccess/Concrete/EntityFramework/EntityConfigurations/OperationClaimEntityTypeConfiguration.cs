
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
    public class OperationClaimEntityTypeConfiguration : BaseEntityTypeConfiguration<OperationClaim>
    {
        public override void Configure(EntityTypeBuilder<OperationClaim> builder)
        {
            base.Configure(builder);
            builder.ToTable("OperationClaims", t => t.HasTrigger("OperationClaimsDeleteTrigger"));
            builder.HasData(new List<OperationClaim>
            {
                new OperationClaim {Id=1, ClaimName = "Admin"},
                new OperationClaim {Id=2, ClaimName = "Users"}
            });
        }
    }
}