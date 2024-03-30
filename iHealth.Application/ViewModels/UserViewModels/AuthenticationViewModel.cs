using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHealth.Core.Application.ViewModels.UserViewModels
{
    public class AuthenticationViewModel
    {
        public bool Success { get; set; }
        public UserViewModel User { get; set; }
    }
}
