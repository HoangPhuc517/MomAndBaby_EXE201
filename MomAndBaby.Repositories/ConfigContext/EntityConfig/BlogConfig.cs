public class BlogConfig : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CreatedTime)
            .IsRequired();

        builder.Property(a => a.UpdatedTime)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(p => p.Content)
            .IsRequired();

        builder.Property(p => p.Image);

        builder.Property(p => p.LikeCount)
            .IsRequired();

        builder.Property(p => p.CommentCount)
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Blogs)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Likes)
            .WithOne(l => l.Blog)
            .HasForeignKey(l => l.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Blog)
            .HasForeignKey(c => c.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Reports)
            .WithOne(r => r.Blog)
            .HasForeignKey(r => r.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
