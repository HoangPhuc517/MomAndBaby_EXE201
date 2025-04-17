namespace MomAndBaby.Repositories.Entities
{
    public class Feedback
    {
        public string? Content { get; set; }
        public int Stars { get; set; }
        public Guid UserId { get; set; }
        public Guid AppointmentId { get; set; }
        public virtual User User { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}
