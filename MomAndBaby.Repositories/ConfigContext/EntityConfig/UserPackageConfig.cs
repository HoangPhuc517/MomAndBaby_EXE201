namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class UserPackageConfig : IEntityTypeConfiguration<UserPackage>
    {
        public void Configure(EntityTypeBuilder<UserPackage> builder)
        {
            builder.ToTable("UserPackages");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(up => up.ExpiryDate)
                .IsRequired();

            builder.Property(up => up.ValidMonths)
                .IsRequired();

            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPackages)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.ServicePackage)
                .WithMany(sp => sp.UserPackages)
                .HasForeignKey(up => up.ServicePackageId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
