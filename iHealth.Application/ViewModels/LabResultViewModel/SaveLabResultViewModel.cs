using System.ComponentModel.DataAnnotations;
namespace iHealth.Core.Application.ViewModels.LabResultViewModel
{
    public class SaveLabResultViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The appointment is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a doctor")]
        public int AppointmentId { get; set; }
        public AppointmentViewModel.AppointmentViewModel Appointment { get; set; }  

        [Required(ErrorMessage = "The laboratory test is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a laboratry test")]
        public int LabTestId { get; set; }

        [Required(ErrorMessage = "The result is required")]
        [DataType(DataType.Text)]
        public string Result { get; set; }

        [Required(ErrorMessage = "The status is required")]
        public bool Completed { get; set; } = false;
    }
}
