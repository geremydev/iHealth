using iHealth.Core.Application.ViewModels.Base;
using iHealth.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.AppointmentViewModel
{
    public class AppointmentViewModel : BaseEntityViewModel
    {
        [Required(ErrorMessage = "The patient is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "The doctor is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "The date of this appointment is required")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        
        [Required(ErrorMessage = "The medical concern is required")]
        [DataType(DataType.MultilineText)]
        public string MedicalConcern { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.PendingConsultation;


        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }

        public virtual ICollection<LabResult>? LabResult { get; set; }
    }

    public enum AppointmentStatus
    {
        PendingConsultation,
        PendingResults,
        Completed
    }
}
