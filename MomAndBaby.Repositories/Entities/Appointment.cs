namespace MomAndBaby.Repositories.Entities
{
    public class Appointment : BaseEntity
    {
        public string Content { get; set; }
        public string Type { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public string Place { get; set; }
        public string? LinkMeet { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ExpertId { get; set; }
        public Guid? FeebackId { get; set; }
        public Guid? JournalId { get; set; }
        public virtual User Customer { get; set; }
        public virtual Expert Expert { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
