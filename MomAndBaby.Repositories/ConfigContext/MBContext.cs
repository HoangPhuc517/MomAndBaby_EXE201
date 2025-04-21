using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MomAndBaby.Repositories.ConfigContext.EntityConfig;
namespace MomAndBaby.Repositories.ConfigContext
{
    public class MBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MBContext(DbContextOptions<MBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatHub> ChatHubs { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserPackage> UserPackages { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Remove name
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            #endregion

            modelBuilder.ApplyConfiguration(new AppointmentConfig());
            modelBuilder.ApplyConfiguration(new BlogConfig());
            modelBuilder.ApplyConfiguration(new ChatMessageConfig());
            modelBuilder.ApplyConfiguration(new ChathubConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new DealConfig());
            modelBuilder.ApplyConfiguration(new ExpertConfig());
            modelBuilder.ApplyConfiguration(new FeedbackConfig());
            modelBuilder.ApplyConfiguration(new JournalConfig());
            modelBuilder.ApplyConfiguration(new LikeConfig());
            modelBuilder.ApplyConfiguration(new NotificationConfig());
            modelBuilder.ApplyConfiguration(new ReportConfig());
            modelBuilder.ApplyConfiguration(new ServicePackageConfig());
            modelBuilder.ApplyConfiguration(new TransactionConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserPackageConfig());

        }
    }
}
