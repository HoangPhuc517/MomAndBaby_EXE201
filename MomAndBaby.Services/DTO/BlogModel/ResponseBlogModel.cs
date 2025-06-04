using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.BlogModel
{
    public class ResponseBlogModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public virtual Guid UserId { get; set; }
        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();
        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }

    public class LikeModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid UserId { get; set; }
    }

    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }

    public class ReportModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }

    }
}
