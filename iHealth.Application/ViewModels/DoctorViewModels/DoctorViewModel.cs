using iHealth.Core.Application.ViewModels.Base;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.ViewModels.DoctorViewModels
{
    public class DoctorViewModel : BaseHealthCareEntityViewModel
    {
        public virtual ICollection<Appointment>? Appointments { get; set; }

    }
}
