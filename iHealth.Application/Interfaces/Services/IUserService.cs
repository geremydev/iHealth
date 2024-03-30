using iHealth.Core.Application.ViewModels.UserViewModels;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User, UserViewModel, SaveUserViewModel>
    {
        public Task<AuthenticationViewModel> Authenticate(LoginViewModel vm);
        public Task<bool> UsernameAlreadyExists(string username);
    }
}
