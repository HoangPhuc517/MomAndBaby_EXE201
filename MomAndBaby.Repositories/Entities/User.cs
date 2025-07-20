using Microsoft.AspNetCore.Identity;
using MomAndBaby.Core.Store;

namespace MomAndBaby.Repositories.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public UserSexEnum Sex { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public virtual Expert? Expert { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<ChatHub> ChatHubsAsFirstUser { get; set; } = new List<ChatHub>();
        public virtual ICollection<ChatHub> ChatHubsAsSecondUser { get; set; } = new List<ChatHub>();
        public string? RefreshToken { get; set; }
        public DateTimeOffset? DateExpireRefreshToken { get; set; }
    }
}
