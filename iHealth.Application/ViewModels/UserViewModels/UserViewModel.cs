using iHealth.Core.Application.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.UserViewModels
{
    public class UserViewModel : BasePersonViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }

    }

    public enum Role
    {
        Admin,
        Secretary
    }
}
