using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHealth.Core.Application.ViewModels.AppointmentViewModel
{
    public class AddAppointmentViewModel
    {
        public List<PatientViewModel> Patients { get; set; }
        public List<DoctorViewModel> Doctors { get; set; }

        [Required(ErrorMessage = "The date of this appointment is required")]
        public DateOnly Date { get; set; }
        [Required(ErrorMessage = "The time of this appointment is required")]
        public TimeOnly Time { get; set; }
        [Required(ErrorMessage = "The medical concern of this appointment is required")]
        public string MedicalConcern { get; set; }
        [Required(ErrorMessage = "You are required to choose a patient")]
        public int ChosenPatientId { get; set; }
        [Required(ErrorMessage = "You are required to choose a doctor")]
        public int ChosenDoctorId { get; set; }

    }
}
