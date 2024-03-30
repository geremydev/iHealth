using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.Base
{
    public class BaseHealthCareEntityViewModel : BasePersonViewModel
    {
        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Identification card is required")]
        [DataType(DataType.Text)]
        public string IdCard { get; set; }

        [Required(ErrorMessage = "Your profile picture is required")]
        public string ImageURL { get; set; }

    }
}
