using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Configuration
{
    internal class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("StartDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.Ignore(X => X.Id);

            builder.HasKey(X => new { X.MemberId, X.PlanId });
        }
    }
}
