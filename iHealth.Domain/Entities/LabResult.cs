using iHealth.Core.Domain.Common;

namespace iHealth.Core.Domain.Entities
{
    public class LabResult : BaseEntity
    {
        public int? AppointmentId { get; set; }
        public int? LabTestId { get; set; }
        public string Result { get; set; }
        public bool Completed { get; set; } = false;
        
        public virtual Appointment? Appointment { get; set; }
        public virtual LabTest? LabTest { get; set; }
    }
}
