using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.BlogModel;

namespace MomAndBaby.Services.Interface
{
    public interface IBlogService
    {
        Task<ResponseBlogModel> CreateBlog(CreateBlogModel model);
        Task<ResponseBlogModel> UpdateBlog(Guid id, CreateBlogModel model);
        Task<ResponseBlogModel> GetBlogById(Guid id);
        Task<Pagination<ResponseBlogModel>> GetPaginationBlogs(
            int pageIndex,
            int pageSize,
            string? searchString,
            bool isDescending,
            DateTimeOffset? startDate, 
            DateTimeOffset? endDate,
            Guid? userId = null,
            BaseEnum? status = null
            );
        Task DeleteBalog(Guid id);
        Task<List<ResponseBlogModel>> GetAllBlogsByUserIdLiked(Guid userId);
        Task<int> CreateLikeBlog(Guid blogId);
        Task<int> DeleteLikeBlog(Guid blogId);
        Task<(CommentModel, int)> CreateCommentBlog(Guid blogId, string content);
        Task<int> DeleteCommentBlog(Guid commentId);
    }
}
