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
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(u => u.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidCheck", "Email like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserPhoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%'");
            });

            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(u => u.Address, AddressBuilder =>
            {
                AddressBuilder.Property(X=> X.Street)
                              .HasColumnName("Street")
                              .HasColumnType("varchar")
                              .HasMaxLength(30);

                AddressBuilder.Property(X=> X.City)
                              .HasColumnName("City")
                              .HasColumnType("varchar")
                              .HasMaxLength(30);
            });
        }
    }
}
