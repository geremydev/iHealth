using iHealth.Core.Application.ViewModels.LabTestViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHealth.Core.Application.ViewModels.AppointmentViewModel
{
    public class ConsultateViewModel
    {
        public List<LabTestViewModel.LabTestViewModel> LabTestsAvailable { get; set; }
        public List <bool> LabTestsIdChosen { get; set; }
        public AppointmentViewModel Appointment {  get; set; }
    }
}
