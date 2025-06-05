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
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.ChatModel;
using MomAndBaby.Services.Interface;

namespace MomAndBaby.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public ChatService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<ResponseChatHup> CreateChatHup(Guid secondUserId, string nameChatHub)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentUserId = _currentUserService.GetUserId();
                var checkName = await _unitOfWork.GenericRepository<ChatHub>()
                                                 .GetFirstOrDefaultAsync(_ => _.NameChatHub == nameChatHub);
                if (checkName != null)
                {
                    throw new BaseException(StatusCodes.Status409Conflict, "ChatHub name already exists");
                }
                var chatHup = new ChatHub
                {
                    NameChatHub = nameChatHub,
                    FirstUserId = Guid.Parse(currentUserId),
                    SecondUserId = secondUserId
                };
                await _unitOfWork.GenericRepository<ChatHub>().InsertAsync(chatHup);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapper.Map<ResponseChatHup>(chatHup);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CreateChatMessage(Guid chatHupId, string content, string type)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentUserId = Guid.Parse( _currentUserService.GetUserId() );
                var chatHup = await _unitOfWork.GenericRepository<ChatHub>()
                    .GetFirstOrDefaultAsync(_ => _.Id == chatHupId);
                if (chatHup is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "ChatHub not found");
                }
                if (chatHup.FirstUserId != currentUserId && chatHup.SecondUserId != currentUserId)
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "You are not a member of this chat hub");
                }
                var chatMessage = new ChatMessage
                {
                    ChatHubId = chatHupId,
                    Content = content,
                    Type = type,
                    SenderId = currentUserId
                };
                await _unitOfWork.GenericRepository<ChatMessage>().InsertAsync(chatMessage);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteChatMessage(Guid chatMessageId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var chatMessage = await _unitOfWork.GenericRepository<ChatMessage>()
                    .GetFirstOrDefaultAsync(_ => _.Id == chatMessageId);
                if (chatMessage is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Chat message not found");
                }
                if (chatMessage.SenderId != Guid.Parse(_currentUserService.GetUserId()))
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "You are not the sender of this message");
                }
                _unitOfWork.GenericRepository<ChatMessage>().Delete(chatMessage);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<ResponseChatHup>> GetAllChatHupsByUserId(Guid userId)
        {
            try
            {
                var chatHups = await _unitOfWork.GenericRepository<ChatHub>()
                    .GetAllAsync(_ => _.FirstUserId == userId || _.SecondUserId == userId, null);
                if (chatHups is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "No chat hubs found for this user.");
                }
                chatHups = chatHups.OrderByDescending(_ => _.CreatedTime).ToList();
                var responseChatHups = _mapper.Map<List<ResponseChatHup>>(chatHups);
                return responseChatHups;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseChatHup> GetChatHupById(Guid chatHupId)
        {
            try
            {
                var chatHup = await _unitOfWork.GenericRepository<ChatHub>()
                    .GetFirstOrDefaultAsync(
                            predicate: _ => _.Id == chatHupId, 
                            includeProperties: "ChatMessages");
                if (chatHup is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "ChatHub not found");
                }
                chatHup.ChatMessages = chatHup.ChatMessages
                                              .OrderByDescending(_ => _.CreatedTime)
                                              .ToList();
                return _mapper.Map<ResponseChatHup>(chatHup);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseChatHup> UpdateChatHup(Guid chatHupId, string name)
        {
            try
            {
                var chatHup = await _unitOfWork.GenericRepository<ChatHub>()
                    .GetFirstOrDefaultAsync(_ => _.Id == chatHupId);
                if (chatHup is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "ChatHub not found");
                }
                chatHup.NameChatHub = name;
                _unitOfWork.GenericRepository<ChatHub>().Update(chatHup);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<ResponseChatHup>(chatHup);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseChatMessage> UpdateChatMessage(Guid chatMessageId, string content)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var chatMessage = await _unitOfWork.GenericRepository<ChatMessage>()
                    .GetFirstOrDefaultAsync(_ => _.Id == chatMessageId);
                if (chatMessage is null)
                {
                    throw new BaseException(StatusCodes.Status404NotFound, "Chat message not found");
                }
                if (chatMessage.SenderId != Guid.Parse(_currentUserService.GetUserId()))
                {
                    throw new BaseException(StatusCodes.Status403Forbidden, "You are not the sender of this message");
                }
                if (chatMessage.Type != ChatMessageTypeEnum.Text.ToString())
                {
                    throw new BaseException(StatusCodes.Status400BadRequest, "Only text messages can be updated!!!");
                }
                chatMessage.Content = content;
                _unitOfWork.GenericRepository<ChatMessage>().Update(chatMessage);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapper.Map<ResponseChatMessage>(chatMessage);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
