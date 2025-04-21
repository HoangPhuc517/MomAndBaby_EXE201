using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;

namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(a => a.Id);

            builder.Property(u => u.FullName)
                .IsRequired();

            builder.Property(u => u.Avatar);

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.Property(u => u.Sex)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserSexEnum)Enum.Parse(typeof(UserSexEnum), v) 
                )
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();

            builder.Property(u => u.CreatedTime)
                .IsRequired();

            builder.Property(u => u.UpdatedTime)
                .IsRequired();

            builder.HasOne(u => u.Expert)
                .WithOne(e => e.User)
                .HasForeignKey<Expert>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserPackages)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Blogs)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Feedbacks)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Journals)
                .WithOne(j => j.User)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.ChatHubsAsFirstUser)
                .WithOne(c => c.FirstUser)
                .HasForeignKey(c => c.FirstUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.ChatHubsAsSecondUser)
                .WithOne(c => c.SecondUser)
                .HasForeignKey(c => c.SecondUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(t => t.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
