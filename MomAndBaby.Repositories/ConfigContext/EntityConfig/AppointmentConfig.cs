using Microsoft.EntityFrameworkCore;

namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(a => a.Content)
                .IsRequired();

            builder.Property(a => a.Type)
                .IsRequired();

            builder.Property(a => a.AppointmentDate)
                .IsRequired();

            builder.Property(a => a.Place)
                .IsRequired();

            builder.Property(a => a.LinkMeet);

            builder.HasOne(a => a.Customer)
                .WithMany(a => a.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Expert)
                .WithMany(a => a.Appointments)
                .HasForeignKey(a => a.ExpertId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Journal)
                .WithMany(a => a.Appointments)
                .HasForeignKey(a => a.JournalId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.Reports)
                .WithOne(a => a.Appointment)
                .HasForeignKey(r => r.AppointmentId);
        }
    }
}
