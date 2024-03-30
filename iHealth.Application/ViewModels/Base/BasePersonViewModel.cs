using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.Base
{
    public class BasePersonViewModel : BaseEntityViewModel
    {
        [Required(ErrorMessage="Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
