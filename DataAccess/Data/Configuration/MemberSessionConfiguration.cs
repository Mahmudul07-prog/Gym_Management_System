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
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Ignore(X => X.Id);

            builder.HasKey(x => new { x.MemberId, x.SessionId});

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
