using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class ServicePackageConfig : IEntityTypeConfiguration<ServicePackage>
    {
        public void Configure(EntityTypeBuilder<ServicePackage> builder)
        {
            builder.ToTable("ServicePackages");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(sp => sp.Name)
                .IsRequired();

            builder.Property(sp => sp.Description)
                .IsRequired();

            builder.Property(sp => sp.Image)
                .IsRequired();

            builder.Property(sp => sp.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(sp => sp.UserPackages)
                .WithOne(up => up.ServicePackage)
                .HasForeignKey(up => up.ServicePackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sp => sp.Deals)
                .WithOne(d => d.ServicePackage)
                .HasForeignKey(d => d.ServicePackageId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
