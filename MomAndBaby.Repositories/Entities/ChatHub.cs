using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class ChatHub : BaseEntity
    {
        public string NameChatHub { get; set; }
        public Guid? FirstUserId { get; set; }
        public Guid? SecondUserId { get; set; }
        public virtual User? FirstUser { get; set; }
        public virtual User? SecondUser { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
