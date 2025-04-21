namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(f => f.Content);

            builder.Property(f => f.Stars)
                .IsRequired();

            builder.HasOne(f => f.User)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(f => f.Appointment)
                .WithOne(a => a.Feedback)
                .HasForeignKey<Feedback>(f => f.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
