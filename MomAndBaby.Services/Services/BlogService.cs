using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.BlogModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ResponseBlogModel> CreateBlog(CreateBlogModel model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();

                var blog = _mapper.Map<Blog>(model);
                blog.UserId = Guid.Parse(userId);

                await _unitOfWork.GenericRepository<Blog>().InsertAsync(blog);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapper.Map<ResponseBlogModel>(blog);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteBalog(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var blog = await _unitOfWork.GenericRepository<Blog>().GetByIdAsync(id);
                if (blog == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Blog not found");
                }
                blog.Status = BaseEnum.Deleted.ToString();
                blog.UpdatedTime = DateTimeOffset.UtcNow;
                _unitOfWork.GenericRepository<Blog>().Update(blog);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ResponseBlogModel> GetBlogById(Guid id)
        {
            try
            {
                var blog = await _unitOfWork.GenericRepository<Blog>().GetFirstOrDefaultAsync(
                    predicate: _ => _.Id == id,
                    includeProperties: "Likes,Comments,Reports"
                    );
                if (blog == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Blog not found");
                }
                var blogModel = _mapper.Map<ResponseBlogModel>(blog);
                return blogModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<ResponseBlogModel>> GetPaginationBlogs(int pageIndex, int pageSize, string? searchString, bool isDescending, DateTimeOffset? startDate, DateTimeOffset? endDate, Guid? userId = null, BaseEnum? status = null)
        {
            try
            {
                var paginationDB = await _unitOfWork.GenericRepository<Blog>().GetPaginationAsync(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    predicate: _ => (string.IsNullOrEmpty(searchString) || _.Content.Contains(searchString))
                                 && (status == null || _.Status == status.ToString())
                                 && (startDate == null || _.CreatedTime.Date >= startDate.Value.Date)
                                 && (endDate == null || _.CreatedTime <= endDate.Value.Date)
                                 && (userId == null || _.UserId == userId),
                    orderBy: _ => _.CreatedTime,
                    isDescending: isDescending
                );
                if (paginationDB == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "No blogs found");
                }
                var paginationResponse = _mapper.Map<Pagination<ResponseBlogModel>>(paginationDB);
                return paginationResponse;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseBlogModel> UpdateBlog(Guid id, CreateBlogModel model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var blog = await _unitOfWork.GenericRepository<Blog>().GetByIdAsync(id);
                if (blog == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Blog not found");
                }
                var userId = _currentUserService.GetUserId();
                if (blog.UserId != Guid.Parse(userId))
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "You do not have permission to update this blog");
                }
                blog = _mapper.Map(model, blog);
                blog.UpdatedTime = DateTimeOffset.UtcNow;
                    
                _unitOfWork.GenericRepository<Blog>().Update(blog);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<ResponseBlogModel>(blog);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<List<ResponseBlogModel>> GetAllBlogsByUserIdLiked(Guid userId)
        {
            try
            {
                var blogs = await _unitOfWork.GenericRepository<Blog>().GetAllAsync(
                    filter: _ => _.Likes.Any(l => l.UserId == userId),
                    includeProperties: null
                );
                if (blogs == null || !blogs.Any())
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "No liked blogs found for this user");
                }
                return _mapper.Map<List<ResponseBlogModel>>(blogs);
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> CreateLikeBlog(Guid blogId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var blog = await _unitOfWork.GenericRepository<Blog>()
                                            .GetFirstOrDefaultAsync(
                                                predicate: _ => _.Id == blogId,
                                                includeProperties: "Likes");
                if (blog == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Blog not found");
                }
                if (blog.Likes.Any(l => l.UserId == Guid.Parse(userId)))
                {
                    throw new BaseException(StatusCodes.Status400BadRequest, "You have already liked this blog");
                }
                var like = new Like
                {
                    BlogId = blogId,
                    UserId = Guid.Parse(userId)
                };
                await _unitOfWork.GenericRepository<Like>().InsertAsync(like);

                blog.LikeCount += 1;
                _unitOfWork.GenericRepository<Blog>().Update(blog);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return blog.LikeCount;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<int> DeleteLikeBlog(Guid blogId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var blog = await _unitOfWork.GenericRepository<Blog>().GetByIdAsync(blogId);
                if (blog == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Blog not found");
                }
                var like = await _unitOfWork.GenericRepository<Like>().GetFirstOrDefaultAsync(
                    predicate: l => l.BlogId == blogId && l.UserId == Guid.Parse(userId)
                );
                if (like == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Like not found");
                }
                _unitOfWork.GenericRepository<Like>().Delete(like);
                blog.LikeCount -= 1;
                _unitOfWork.GenericRepository<Blog>().Update(blog);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return blog.LikeCount;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<(CommentModel, int)> CreateCommentBlog(Guid blogId, string content)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var blog = await _unitOfWork.GenericRepository<Blog>().GetByIdAsync(blogId);
                if (blog == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Blog not found");
                }
                var comment = new Comment
                {
                    BlogId = blogId,
                    UserId = Guid.Parse(userId),
                    Content = content
                };
                await _unitOfWork.GenericRepository<Comment>().InsertAsync(comment);

                blog.CommentCount += 1;
                _unitOfWork.GenericRepository<Blog>().Update(blog);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var commentModel = _mapper.Map<CommentModel>(comment);
                return (commentModel, blog.CommentCount);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<int> DeleteCommentBlog(Guid commentId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var comment = await _unitOfWork.GenericRepository<Comment>().GetByIdAsync(commentId);
                if (comment == null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Comment not found");
                }
                if (comment.UserId != Guid.Parse(userId))
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "You do not have permission to delete this comment");
                }
                var blog = await _unitOfWork.GenericRepository<Blog>().GetByIdAsync(comment.BlogId);
                blog.CommentCount -= 1;
                _unitOfWork.GenericRepository<Blog>().Update(blog);
                _unitOfWork.GenericRepository<Comment>().Delete(comment);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return blog.CommentCount;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
