using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
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
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FeedbackViewModel> CreateFeeback(CreateFeedbackDTO model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var appointment = await _unitOfWork.GenericRepository<Appointment>()
                                                   .GetByIdAsync(model.AppointmentId);
                if (appointment == null) throw new BaseException(StatusCodes.Status404NotFound, "Appointment not found");

                if (appointment.CustomerId.ToString() != userId) throw new BaseException(StatusCodes.Status403Forbidden, "You are not allowed to submit feedback for this appointment");

                if (appointment.Status != AppointmentStatusEnum.Completed.ToString())
                    throw new BaseException(StatusCodes.Status400BadRequest, "You can only submit feedback for completed appointments");

                if (appointment.Feedback != null) throw new BaseException(StatusCodes.Status409Conflict, "You have already submitted feedback!!!");

                var feedback = new Feedback
                {
                    Content = model.Content,
                    Stars = model.Stars,
                    UserId = Guid.Parse(userId),
                    AppointmentId = model.AppointmentId
                };

                await _unitOfWork.GenericRepository<Feedback>()
                                 .InsertAsync(feedback);

                appointment.FeebackId = feedback.Id;
                _unitOfWork.GenericRepository<Appointment>().Update(appointment);

                var expert = await _unitOfWork.GenericRepository<Expert>()
                                              .GetByIdAsync(appointment.ExpertId);

                expert.Stars = expert.Stars is null 
                              ? feedback.Stars 
                              : (expert.Stars + feedback.Stars) / 2;

                _unitOfWork.GenericRepository<Expert>().Update(expert);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var feedbackVM = _mapper.Map<FeedbackViewModel>(feedback);
                return feedbackVM;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<FeedbackViewModel> GetFeedbackById(Guid FeedbackId)
        {
            try
            {
                var feedback = await _unitOfWork.GenericRepository<Feedback>()
                                      .GetByIdAsync(FeedbackId);

                if (feedback is null) throw new BaseException(StatusCodes.Status404NotFound, "Feedback not found");

                var feedbackVM = _mapper.Map<FeedbackViewModel>(feedback);

                return feedbackVM;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<FeedbackViewModel>> GetFeedbackOfExpert(int pageSize, int pageIndex, bool isDescending, Guid ExpertId)
        {
            try
            {
                var feedbacks = await _unitOfWork.GenericRepository<Feedback>()
                                           .GetPaginationAsync(
                                            pageIndex: pageIndex,
                                            pageSize: pageSize,
                                            orderBy: _ => _.CreatedTime,
                                            isDescending: isDescending,
                                            predicate: _ => _.Appointment.ExpertId == ExpertId,
                                            includeProperties: null
                                            );
                if (feedbacks.Items is null) throw new BaseException(StatusCodes.Status404NotFound, "Feedback not found!!!");

                var feedbackVMs = _mapper.Map<Pagination<FeedbackViewModel>>(feedbacks);
                return feedbackVMs;
            }
            catch
            {
                throw;
            }
        }

        public async Task<FeedbackViewModel> UpdateFeeback(UpdateFeedbackDTO updateModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();

                var feedback = await _unitOfWork.GenericRepository<Feedback>()
                                                .GetByIdAsync(updateModel.FeedbackId);
                if (feedback is null) throw new BaseException(StatusCodes.Status404NotFound, "Feedback not found");

                if (feedback.UserId.ToString() != userId) throw new BaseException(StatusCodes.Status403Forbidden, "You are not allowed to update this feedback");

                feedback.Content = updateModel.Content;

                _unitOfWork.GenericRepository<Feedback>().Update(feedback);



                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var feedbackVM = _mapper.Map<FeedbackViewModel>(feedback);

                return feedbackVM;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
