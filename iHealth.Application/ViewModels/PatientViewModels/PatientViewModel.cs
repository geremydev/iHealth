using iHealth.Core.Application.ViewModels.Base;
using iHealth.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.PatientViewModels
{
    public class PatientViewModel : BaseHealthCareEntityViewModel
    {
        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required(ErrorMessage = "This information is required")]
        public bool Smoker { get; set; }

        [Required(ErrorMessage = "This information is required")]
        public bool Allergies { get; set; }


        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
