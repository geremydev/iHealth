using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.PatientViewModels
{
    public class SavePatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Identification card is required")]
        [DataType(DataType.Text)]
        public string IdCard { get; set; }

        [DataType(DataType.Text)]
        public string ImageURL { get; set; } = "";
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required(ErrorMessage = "This information is required")]
        public bool Smoker { get; set; } = false;

        [Required(ErrorMessage = "This information is required")]
        public bool Allergies { get; set; } = false;
    }
}
