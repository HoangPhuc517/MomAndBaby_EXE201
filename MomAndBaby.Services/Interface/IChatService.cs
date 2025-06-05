using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Services.DTO.ChatModel;

namespace MomAndBaby.Services.Interface
{
    public interface IChatService
    {
        Task<ResponseChatHup> CreateChatHup(Guid SecondUserId, string nameChatHub);
        Task<List<ResponseChatHup>> GetAllChatHupsByUserId(Guid userId);
        Task<ResponseChatHup> GetChatHupById(Guid chatHupId);
        Task<ResponseChatHup> UpdateChatHup(Guid chatHupId, string name);
        Task CreateChatMessage(Guid chatHupId, string content, string type);
        Task DeleteChatMessage(Guid chatMessageId);
        Task<ResponseChatMessage> UpdateChatMessage(Guid chatMessageId, string content);
    }
}
