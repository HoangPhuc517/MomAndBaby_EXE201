using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.AppointmentModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService mailService, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailService = mailService;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<AppointmentViewModel> CreateAppointment(CreateAppointmentDTO appointment)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var dateNow = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));

                var userExpert = await _unitOfWork.GenericRepository<User>()
                                                  .GetFirstOrDefaultAsync(_ => _.Expert.Id == appointment.ExpertId, "Expert");
                var timeSlot = await _unitOfWork.GenericRepository<TimeSlot>()
                                                    .GetFirstOrDefaultAsync(_ => _.Id == appointment.TimeSlotId);
                if (userExpert is null) throw new BaseException(StatusCodes.Status404NotFound, "Expert not found!!!");

                if (dateNow.Date == appointment.AppointmentDate.Date)
                {

                    var (startTime, endTime) = TimeOnlyProccess.ConvertToTimeOnlyRange(timeSlot.Time);

                    var checkTime = appointment.AppointmentDate.Date.Add(startTime.ToTimeSpan());

                    if (dateNow.Add(TimeSpan.FromHours(3)) > checkTime)
                    {
                        throw new BaseException(StatusCodes.Status400BadRequest, "You cannot create an appointment within 2 hour of the current time!!!");
                    }
                }
                else if (appointment.AppointmentDate.Date < dateNow.Date)
                {
                    throw new BaseException(StatusCodes.Status400BadRequest, "You cannot create an appointment in the past!!!");
                }

                var userpackage = await _unitOfWork.GenericRepository<UserPackage>()
                                  .GetFirstOrDefaultAsync
                                  (
                                    _ => _.UserId == appointment.CustomerId
                                            && _.Status == BaseEnum.Active.ToString());
                
                if (userpackage is null || userpackage.ExpiryDate.Date < dateNow.Date || appointment.AppointmentDate.Date > userpackage.ExpiryDate.Date)
                {
                    throw new BaseException(StatusCodes.Status400BadRequest, "User package is not valid or expired!!!");
                }


                if (userpackage.UsageCount != null)
                {
                    var numberOfMonth = (appointment.AppointmentDate.Year * 12 + appointment.AppointmentDate.Month)
                                  - (userpackage.CreatedTime.Year * 12 + userpackage.CreatedTime.Month);
                    if (dateNow.Day >= userpackage.CreatedTime.Day)
                    {
                        ++numberOfMonth;
                    }

                    var startDate = userpackage.CreatedTime.AddMonths(numberOfMonth - 1);
                    var endDate = startDate.AddMonths(1);

                    var appointmentCustomerList = await _unitOfWork.GenericRepository<Appointment>()
                        .GetAllAsync(
                            _ => _.CustomerId == appointment.CustomerId
                                 && _.CreatedTime >= startDate
                                 && _.AppointmentDate < endDate
                                 && _.Status != AppointmentStatusEnum.Canceled.ToString(),
                            null
                        );

                    if (appointmentCustomerList.Count() >= userpackage.UsageCount)
                    {
                        throw new BaseException(StatusCodes.Status400BadRequest, "User package usage count exceeded!!!");
                    }
                }




                var check = await _unitOfWork.GenericRepository<Appointment>()
                    .GetFirstOrDefaultAsync
                    (
                        _ => (_.CustomerId == appointment.CustomerId
                               || _.ExpertId == appointment.ExpertId)
                          && _.AppointmentDate == appointment.AppointmentDate
                          && _.TimeSlotId == appointment.TimeSlotId
                          && _.Status != AppointmentStatusEnum.Canceled.ToString()
                    );
                if (check != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "Schedule conflict detected for the Expert or Customer!!!");
                }

                var appointmentEntity = _mapper.Map<Appointment>(appointment);
                appointmentEntity.Status = AppointmentStatusEnum.Pending.ToString();

                appointmentEntity.TimeSlot = timeSlot;

                appointmentEntity.Journal = new Journal
                {
                    Head = $"{appointment.Type.ToString().ToUpper()} with Expert: {userExpert.FullName}",
                    Content = appointment.Content,
                    UserId = appointment.CustomerId
                };

                await _unitOfWork.GenericRepository<Appointment>()
                                 .InsertAsync(appointmentEntity);

                //Notification
                var customerNotification = new Notification
                {
                    Message = appointmentEntity.Type,
                    Type = NotificationTypeEnum.Appointment.ToString(),
                    UserId = appointment.CustomerId,
                };


                
                var expertNotification = new Notification
                {
                    Message = appointmentEntity.Type,
                    Type = NotificationTypeEnum.Appointment.ToString(),
                    UserId = userExpert.Id,
                };

                await _unitOfWork.GenericRepository<Notification>()
                                 .InsertAsync(customerNotification);
                await _unitOfWork.GenericRepository<Notification>()
                                 .InsertAsync(expertNotification);


                //Send Email
                

                await _mailService.SendMailAppointmentNotification(appointmentEntity, userExpert.NormalizedEmail);

                var result = _mapper.Map<AppointmentViewModel>(appointmentEntity);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<AppointmentViewModel> GetAppointmentById(string id)
        {
            try
            {
                var appointment = await _unitOfWork.GenericRepository<Appointment>()
                                                   .GetFirstOrDefaultAsync(_ => _.Id.ToString() == id,
                                                       "TimeSlot,Feedback,Reports"
                                                   );
                if (appointment is null) throw new BaseException(StatusCodes.Status404NotFound, "Appointment not found!!!");

                var appointmentViewModel = _mapper.Map<AppointmentViewModel>(appointment);
                appointmentViewModel.TimeSlot = appointment.TimeSlot.Time;
                appointmentViewModel.ReportCount = appointment.Reports.Count();

                return appointmentViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<AppointmentViewModel>> GetAppointmentByPagination(int pageIndex, int pageSize, string? searchString, string? userId, AppointmentStatusEnum? appointmentStarusEnum, AppointmentTypeEnum? appointmentTypeEnum, bool isDescending)
        {
            try
            {
                var paging = await _unitOfWork.GenericRepository<Appointment>()
                       .GetPaginationAsync(
                    predicate: _ =>
                       (
                            (string.IsNullOrEmpty(searchString)
                            || _.Type.Contains(searchString)
                            || _.Content.Contains(searchString))
                            && (string.IsNullOrEmpty(userId)
                                || _.CustomerId.ToString() == userId
                                || _.ExpertId.ToString() == userId)
                            && (appointmentStarusEnum == null
                                || _.Status == appointmentStarusEnum.ToString())
                            && (appointmentTypeEnum == null
                                || _.Type == appointmentTypeEnum.ToString())
                       ),
                    includeProperties: "TimeSlot,Feedback,Reports",
                    orderBy: _ => _.CreatedTime,
                    isDescending: isDescending,
                    pageIndex: pageIndex,
                    pageSize: pageSize
                       );
                var result = _mapper.Map<Pagination<AppointmentViewModel>>(paging);
                return result;

            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateStatusAppointment(string id, AppointmentStatusEnum statusEnum)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var appointment = await _unitOfWork.GenericRepository<Appointment>()
                                                   .GetFirstOrDefaultAsync(_ => _.Id.ToString() == id, "TimeSlot");

                if (appointment is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Appointment not found!!!");
                }

                var userId = _currentUserService.GetUserId();
                var user = await _unitOfWork.GenericRepository<User>()
                                            .GetFirstOrDefaultAsync(_ => _.Id.ToString() == userId,
                                                                            "Expert");
                var roleCurrentUsers = await _userManager.GetRolesAsync(user);
                var roleCurrentUser = roleCurrentUsers.FirstOrDefault();

                AppointmentHelper.CheckValidUserUpdateAppointment(roleCurrentUser, statusEnum);

                AppointmentHelper.CheckIsCancelCompleteAppointment(appointment);


                if (userId == appointment.CustomerId.ToString() || user.Expert?.Id == appointment.ExpertId)
                {
                    if (statusEnum.Equals(AppointmentStatusEnum.Canceled))
                    {
                        var numberDays = (appointment.AppointmentDate.Date - DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)).Date).TotalDays;
                        if (numberDays < 2)
                        {
                            throw new BaseException(StatusCodes.Status400BadRequest, "You cannot cancel the appointment less than 48 hours before the scheduled time!!!");
                        }
                    }

                    appointment.Status = statusEnum.ToString();


                    var roleUser = await _userManager.GetRolesAsync(user);
                    var role = roleUser.FirstOrDefault();


                    if (role == UserRoleEnum.CUSTOMER.ToString() && statusEnum == AppointmentStatusEnum.Canceled)
                    {
                        var expert = await _unitOfWork.GenericRepository<Expert>()
                                                      .GetFirstOrDefaultAsync(_ => _.Id == appointment.ExpertId,
                                                          "User"
                                                      );
                        await _mailService.SendMailAppointmentNotification(appointment, expert.User.NormalizedEmail);
                    }
                    else if (role == UserRoleEnum.EXPERT.ToString())
                    {
                        var customer = await _unitOfWork.GenericRepository<User>()
                                                        .GetFirstOrDefaultAsync(_ => _.Id.ToString() == appointment.CustomerId.ToString()
                                                      );
                        await _mailService.SendMailAppointmentNotification(appointment, customer.NormalizedEmail);
                    }

                    _unitOfWork.GenericRepository<Appointment>()
                                     .Update(appointment);
                    await _unitOfWork.SaveChangeAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "You do not have permission to update this appointment status!!!");
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<TimeSlot> CreateTimeSlot(TimeOnly startTime, TimeOnly endTime)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                var time = TimeOnlyProccess.ConvertToString(startTime, endTime);

                var timeSlotEntity = new TimeSlot
                {
                    Time = time
                };

                await _unitOfWork.GenericRepository<TimeSlot>()
                                 .InsertAsync(timeSlotEntity);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return timeSlotEntity;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<List<TimeSlot>> GetTimeSlots()
        {
            try
            {
                var timeSlotList = await _unitOfWork.GenericRepository<TimeSlot>()
                                                    .GetAllAsync(_ => _.Status == BaseEnum.Active.ToString(),
                                                                    null);
                if (timeSlotList is null) throw new BaseException(StatusCodes.Status404NotFound, "Time slot not found!!!");
                return timeSlotList;
            }
            catch
            {
                throw;
            }
        }
    }
}
