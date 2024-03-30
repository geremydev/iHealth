using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Repositories.Persons;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UserInMemoryDatabase"))
            {
                services.AddDbContext<iHealthContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<iHealthContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DeffaultConnection"),
                m => m.MigrationsAssembly(typeof(iHealthContext).Assembly.FullName)));
            }
            #endregion
            #region Repositories  
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<ILabResultRepository, LabResultRepository>();
            services.AddTransient<ILabTestRepository, LabTestRepository>();
            #endregion
        }
    }
}
