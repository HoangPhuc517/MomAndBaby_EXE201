using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.FeedbackModel;

namespace MomAndBaby.Services.Interface
{
    public interface IFeedbackService
    {
        public Task<FeedbackViewModel> CreateFeeback(CreateFeedbackDTO model);
        public Task<FeedbackViewModel> UpdateFeeback(CreateFeedbackDTO updateModel);
        public Task<Pagination<FeedbackViewModel>> GetFeedbackOfExpert(Guid ExpertId);
        public Task<FeedbackViewModel> GetFeedbackById(Guid FeedbackId);
    }
}
