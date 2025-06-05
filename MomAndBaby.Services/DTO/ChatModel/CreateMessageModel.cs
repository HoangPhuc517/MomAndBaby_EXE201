using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;

namespace MomAndBaby.Services.DTO.ChatModel
{
    public class CreateMessageModel
    {
        [Required]
        public Guid ChatHubId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        [Required]
        public ChatMessageTypeEnum Type { get; set; }
    }
}
