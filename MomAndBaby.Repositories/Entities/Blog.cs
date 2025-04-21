namespace MomAndBaby.Repositories.Entities
{
    public class Blog : BaseEntity
    {
        public string Content { get; set; }
        public string? Image { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
