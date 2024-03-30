using iHealth.Core.Domain.Base;
namespace iHealth.Core.Domain.Entities
{
    public class Doctor : HealthcareEntity
    {
        public virtual ICollection<Appointment>? Appointments { get; set; }

    }
}
