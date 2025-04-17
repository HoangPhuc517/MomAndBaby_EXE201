using Microsoft.AspNetCore.Identity;

namespace MomAndBaby.Repositories.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string DateOfBirth { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
