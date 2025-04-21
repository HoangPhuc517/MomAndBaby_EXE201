namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class DealConfig : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.ToTable("Deals");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(a => a.Image);

            builder.Property(p => p.DiscountRate)
                .IsRequired();

            builder.Property(p => p.EndDate)
                .IsRequired();

            builder.Property(p => p.OfferConditions)
                .IsRequired();

            builder.HasOne(p => p.ServicePackage)
                .WithMany(p => p.Deals)
                .HasForeignKey(p => p.ServicePackageId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
