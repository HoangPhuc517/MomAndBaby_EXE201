namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class ReportConfig : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(r => r.Content)
                .IsRequired();

            builder.HasOne(r => r.User)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Blog)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.BlogId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Appointment)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
