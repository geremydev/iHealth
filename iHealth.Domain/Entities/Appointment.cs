using iHealth.Core.Domain.Common;

namespace iHealth.Core.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string MedicalConcern { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.PendingConsultation;
    
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<LabResult>? LabResult { get; set; }

    }

    public enum AppointmentStatus
    {
        PendingConsultation,
        PensungResults,
        Completed
    }
}
