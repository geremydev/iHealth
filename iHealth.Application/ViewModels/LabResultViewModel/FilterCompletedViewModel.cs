using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHealth.Core.Application.ViewModels.LabResultViewModel
{
    public class FilterCompletedViewModel
    {
        public FilterViewModel Filter = new FilterViewModel();
        public List<LabResultViewModel> LabResults {  get; set; }

        public List<PatientViewModels.PatientViewModel> Patients { get; set; }
        public List<DoctorViewModels.DoctorViewModel> Doctors { get; set; }

    }
}
