using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.JournalModel;

namespace MomAndBaby.Services.Interface
{
    public interface IJournalService
    {
        Task<JournalViewModel> CreateJournal(JournalDto journalDto);
        Task<JournalViewModel> UpdateJournal(string journalId, JournalDto journalDto);
        Task DeleteJournal(string journalId);
        Task<Pagination<JournalViewModel>> GetJournals(
            int pageIndex,
            int pageSize,
            string? headOrContent,
            bool isDescending
            );
        Task<List<JournalViewModel>> GetJournalsByPeriod(DateTime startDate, DateTime endDate);
    }
}
