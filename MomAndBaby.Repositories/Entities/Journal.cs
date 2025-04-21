namespace MomAndBaby.Repositories.Entities
{
    public class Journal : BaseEntity
    {
        public string Head { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
