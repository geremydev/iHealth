using iHealth.Core.Domain.Base;

namespace iHealth.Core.Domain.Entities
{
    public class Patient : HealthcareEntity
    {
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Smoker { get; set; }
        public bool Allergies { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
