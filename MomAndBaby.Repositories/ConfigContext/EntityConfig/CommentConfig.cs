
namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(c => c.Content)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Reports)
                .WithOne(c => c.Comment)
                .HasForeignKey(c => c.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
