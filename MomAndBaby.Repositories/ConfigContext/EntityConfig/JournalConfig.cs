namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class JournalConfig : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder.ToTable("Journals");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(j => j.Head)
                .IsRequired();

            builder.Property(j => j.Content);

            builder.Property(j => j.Image);

            builder.HasOne(j => j.User)
                .WithMany(j => j.Journals)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.Appointments)
                .WithOne(a => a.Journal)
                .HasForeignKey(a => a.JournalId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
