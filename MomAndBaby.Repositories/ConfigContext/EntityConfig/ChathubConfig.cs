namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class ChathubConfig : IEntityTypeConfiguration<ChatHub>
    {
        public void Configure(EntityTypeBuilder<ChatHub> builder)
        {
            builder.ToTable("ChatHubs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(c => c.NameChatHub)
                .IsRequired();

            builder.HasMany(c => c.ChatMessages)
                .WithOne(m => m.ChatHub)
                .HasForeignKey(m => m.ChatHubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
