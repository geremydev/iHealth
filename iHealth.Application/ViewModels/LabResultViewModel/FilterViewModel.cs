using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHealth.Core.Application.ViewModels.LabResultViewModel
{
    public class FilterViewModel
    {
        public int? PatientId { get; set; }
        public string? PatientIdCard { get; set; }
        public int? DoctorId { get; set; }
    }
}
