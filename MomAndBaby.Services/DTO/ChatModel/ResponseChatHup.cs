﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.DTO.ChatModel
{
    public class ResponseChatHup
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string NameChatHub { get; set; }
        public Guid? FirstUserId { get; set; }
        public Guid? SecondUserId { get; set; }
        public virtual ICollection<ResponseChatMessage> ChatMessages { get; set; } = new List<ResponseChatMessage>();
    }

    public class ResponseChatMessage
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public Guid SenderId { get; set; }
    }
}
