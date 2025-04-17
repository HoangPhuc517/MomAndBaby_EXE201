namespace MomAndBaby.Repositories.Entities
{
    public class Expert : BaseEntity
    {
        public string Degree { get; set; }
        public string? Workplace { get; set; }
        public string? Description { get; set; }
        public string Specialty { get; set; }
        public double Stars { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
