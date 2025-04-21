using System;
namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class ExpertConfig : IEntityTypeConfiguration<Expert>
    {
        public void Configure(EntityTypeBuilder<Expert> builder)
        {
            builder.ToTable("Experts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(e => e.Degree)
                .IsRequired();

            builder.Property(e => e.Specialty)
                .IsRequired();

            builder.Property(e => e.Stars)
                .HasColumnType("decimal(1,1)");

            builder.HasOne(e => e.User)
                .WithOne(e => e.Expert)
                .HasForeignKey<Expert>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Appointments)
                .WithOne(a => a.Expert)
                .HasForeignKey(a => a.ExpertId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
