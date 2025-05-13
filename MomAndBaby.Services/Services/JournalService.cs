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
using MomAndBaby.Services.DTO.JournalModel;
using MomAndBaby.Services.Helpers;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class JournalService : IJournalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UploadFile _hepperUploadImage;
        public readonly IMapper _mapper;

        public JournalService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UploadFile uploadFile, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _hepperUploadImage = uploadFile;
            _mapper = mapper;
        }

        public async Task<JournalViewModel> CreateJournal(JournalDto journalDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUserService.GetUserId();
                var journal = _mapper.Map<Journal>(journalDto);
                journal.Status = BaseEnum.Active.ToString();
                journal.UserId = Guid.Parse(userId);

                if (journalDto.Image != null)
                {
                    using var stream = journalDto.Image.OpenReadStream();
                    var url = await _hepperUploadImage.UploadImageAsync(stream, journalDto.Image.FileName);
                    journal.Image = url;
                }

                await _unitOfWork.GenericRepository<Journal>().InsertAsync(journal);
                await _unitOfWork.SaveChangeAsync();
                var journalViewModel = _mapper.Map<JournalViewModel>(journal);
                await _unitOfWork.CommitTransactionAsync();

                return journalViewModel;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task DeleteJournal(string journalId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var journal = await _unitOfWork.GenericRepository<Journal>()
                                           .GetFirstOrDefaultAsync(_ => _.Id.ToString() == journalId);
                if (journal is null) throw new BaseException(StatusCodes.Status404NotFound, "Journal not found");
                journal.Status = BaseEnum.Deleted.ToString();

                _unitOfWork.GenericRepository<Journal>().Update(journal);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }
        public async Task<Pagination<JournalViewModel>> GetJournals(int pageIndex, int pageSize, string? headOrContent, bool isDescending)
        {
            try
            {
                var userId = _currentUserService.GetUserId();

                var journalDbList = await _unitOfWork.GenericRepository<Journal>()
                                                     .GetPaginationAsync(
                                                        predicate: _ =>
                                                            _.Status == BaseEnum.Active.ToString()
                                                            && (string.IsNullOrEmpty(headOrContent)
                                                                || _.Head.Contains(headOrContent)
                                                                || _.Content.Contains(headOrContent))
                                                            && _.UserId.ToString() == userId,
                                                        pageIndex: pageIndex,
                                                        pageSize: pageSize,
                                                        isDescending: isDescending,
                                                        includeProperties: null,
                                                        orderBy: _ => _.CreatedTime
    );

                if (journalDbList is null) throw new BaseException(StatusCodes.Status404NotFound, "Journal List Empty!!!");
                var journalViewModelList = _mapper.Map<Pagination<JournalViewModel>>(journalDbList);
                return journalViewModelList;
            }
            catch
            {
                throw;
            }
        }
        public async Task<JournalViewModel> UpdateJournal(string journalId, JournalDto journalDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var journal = await _unitOfWork.GenericRepository<Journal>()
                                           .GetFirstOrDefaultAsync(_ => _.Id.ToString() == journalId);

                if (journal is null) throw new BaseException(StatusCodes.Status404NotFound, "Journal not found");

                string? imageUrl = journal.Image;

                _mapper.Map(journalDto, journal);

                if (journalDto.Image != null)
                {
                    using var stream = journalDto.Image.OpenReadStream();
                    imageUrl = await _hepperUploadImage.UploadImageAsync(stream, journalDto.Image.FileName);
                }

                journal.Image = imageUrl;
                journal.UpdatedTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));

                _unitOfWork.GenericRepository<Journal>().Update(journal);
                await _unitOfWork.SaveChangeAsync();
                var journalViewModel = _mapper.Map<JournalViewModel>(journal);
                await _unitOfWork.CommitTransactionAsync();
                return journalViewModel;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<JournalViewModel>> GetJournalsByPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                var journalDbList = await _unitOfWork.GenericRepository<Journal>()
                                                     .GetAllAsync(_ => _.Status == BaseEnum.Active.ToString()
                                                                       && _.CreatedTime.Date >= startDate.Date
                                                                       && _.CreatedTime.Date <= endDate.Date
                                                                       && _.UserId.ToString() == userId,
                                                                       null);
                if (journalDbList is null) throw new BaseException(StatusCodes.Status404NotFound, "Journal List Empty!!!");
                var journalViewModelList = _mapper.Map<List<JournalViewModel>>(journalDbList)
                                                  .OrderBy(_ => _.CreatedTime)
                                                  .ToList();
                return journalViewModelList;
            }
            catch
            {
                throw;
            }
        }
    }
}
