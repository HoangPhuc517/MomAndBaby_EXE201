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

                if (appointment.Feedback != null) throw new BaseException(StatusCodes.Status409Conflict, "You have already submitted feedback!!!");

                appointment.Feedback = new Feedback
                {
                    Content = model.Content,
                    Stars = model.Stars,
                    UserId = Guid.Parse(userId),
                    AppointmentId = model.AppointmentId
                };

                _unitOfWork.GenericRepository<Appointment>().Update(appointment);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                var feedback = await _unitOfWork.GenericRepository<Feedback>()
                                                  .GetByIdAsync(appointment.Feedback.Id);
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

        public Task<Pagination<FeedbackViewModel>> GetFeedbackOfExpert(Guid ExpertId)
        {
            throw new NotImplementedException();
        }

        public async Task<FeedbackViewModel> UpdateFeeback(CreateFeedbackDTO updateModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var appointment = await _unitOfWork.GenericRepository<Appointment>()
                                                   .GetByIdAsync(updateModel.AppointmentId);
                if (appointment == null) throw new BaseException(StatusCodes.Status404NotFound, "Appointment not found");

                if (appointment.CustomerId.ToString() != userId) throw new BaseException(StatusCodes.Status403Forbidden, "You are not allowed to submit feedback for this appointment");


            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
