using iHealth.Core.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;

namespace iHealth.Middleware
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ValidateUserSession(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool HasUser()
        {
            UserViewModel uvm = _contextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            if (uvm == null)
                return false;
            return true;
        }
    }
}
