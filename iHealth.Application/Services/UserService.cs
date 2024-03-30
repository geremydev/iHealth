using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.UserViewModels;
using iHealth.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;

namespace iHealth.Core.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            UserViewModel User = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var model = await VMToModel(await SaveToVM(vm));
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Name;
            model = await _userRepository.SaveAsync(model);
            return await VMToSave(await ModToViewModel(model));
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            var list = await _userRepository.GetAllAsync();
            var newlist = new List<UserViewModel>();

            foreach (var element in list)
            {
                newlist.Add(await ModToViewModel(element));
            };

            newlist = newlist.FindAll(e => e.Deleted == false);
            return newlist;
        }

        public async Task<SaveUserViewModel> GetById(int Id)
        {
            return await VMToSave(await ModToViewModel(await _userRepository.GetEntityByIDAsync(Id)));
        }

        public async Task Update(SaveUserViewModel vm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _userRepository.GetEntityByIDAsync(vm.Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            entity.Name = vm.Name;
            entity.LastName = vm.LastName;
            entity.UserName = vm.UserName;
            entity.Email = vm.Email;
            if(vm.Password != null) entity.Password = vm.Password;
            await _userRepository.UpdateAsync(entity, entity.Id);
        }

        public async Task Delete(int Id)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _userRepository.GetEntityByIDAsync(Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            await _userRepository.DeleteAsync(entity);
        }

        public async Task<bool> UsernameAlreadyExists(string username)
        {
            var list = await _userRepository.GetAllAsync();
            if (list.Any(e => e.UserName == username))
            {
                return true;
            }
            return false;
        }




        
        public async Task<AuthenticationViewModel> Authenticate(LoginViewModel vm)
        {
            var result = await _userRepository.FindAllAsync(u => u.UserName == vm.UserName && u.Password == vm.Password);
            if (result.Count > 0)
                return new AuthenticationViewModel()
                {
                    User = await ModToViewModel(result[0]),
                    Success = true
                };
            else
                return new AuthenticationViewModel()
                {
                    User = null,
                    Success = false
                };
        }

        







        public async Task<SaveUserViewModel> VMToSave(UserViewModel vm)
        {
            return new SaveUserViewModel()
            {
                Email = vm.Email,
                Id = vm.Id,
                LastName = vm.LastName,
                Name = vm.Name,
                Password = vm.Password,
                Role = vm.Role,
                UserName = vm.UserName
            };
        }

        public async Task<UserViewModel> SaveToVM(SaveUserViewModel sm)
        {
            return new UserViewModel()
            {
                Email = sm.Email,
                Id = sm.Id,
                LastName = sm.LastName,
                Name = sm.Name,
                Password = sm.Password,
                Role = sm.Role,
                UserName = sm.UserName
            };
        }

        public async Task<User> VMToModel(UserViewModel vm)
        {
            return new User()
            {
                Id = vm.Id,
                Name = vm.Name,
                Email = vm.Email,
                LastName = vm.LastName,
                Role = (Domain.Entities.Role)vm.Role,
                UserName = vm.UserName,
                Password = vm.Password,
                
                //Auditory
                CreatedBy = vm.CreatedBy,
                CreatedDate = vm.CreatedDate,
                DeletedBy = vm.DeletedBy,
                DeletedDate = vm.DeletedDate,
                Deleted = vm.Deleted,
                LastModifiedBy = vm.LastModifiedBy,
                LastModifiedDate = vm.LastModifiedDate
            };
        }
        public async Task<UserViewModel> ModToViewModel(User m)
        {
            return new UserViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                LastName = m.LastName,
                Role = (ViewModels.UserViewModels.Role)m.Role,
                UserName = m.UserName,
                Password = m.Password,
                //Auditory
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                DeletedBy = m.DeletedBy,
                DeletedDate = m.DeletedDate,
                Deleted = m.Deleted,
                LastModifiedBy = m.LastModifiedBy,
                LastModifiedDate = m.LastModifiedDate
            };
        }
    }
}