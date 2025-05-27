using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.FeedbackModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class FeedbackService : IFeedbackService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public FeedbackService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<FeedbackViewModel> CreateFeeback(CreateFeedbackDTO model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _currentUserService.GetCurrentAccountAsync();
                return null;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public Task<FeedbackViewModel> GetFeedbackById(Guid FeedbackId)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<FeedbackViewModel>> GetFeedbackOfExpert(Guid ExpertId)
        {
            throw new NotImplementedException();
        }

        public Task<FeedbackViewModel> UpdateFeeback(CreateFeedbackDTO updateModel)
        {
            throw new NotImplementedException();
        }
    }
}
