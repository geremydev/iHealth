using iHealth.Core.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.Services;

namespace iHealth.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceApplication(this IServiceCollection services)
        {
            #region Repositories  
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<ILabResultService, LabResultService>();
            services.AddTransient<ILabTestService, LabTestService>();
            #endregion
        }
    }
}
