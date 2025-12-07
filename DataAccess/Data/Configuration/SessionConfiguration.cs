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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapcityCheck", "Capacity Between 1 and 25");
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            // Session Category
            builder.HasOne(S => S.SessionCategory)
                   .WithMany(S => S.Sessions)
                   .HasForeignKey(S => S.CategoryId);

            // Session Trainer
            builder.HasOne(S => S.SessionTrainer)
                   .WithMany(S => S.TrainerSessions)
                   .HasForeignKey(S => S.TrainerId);
        }
    }
}
