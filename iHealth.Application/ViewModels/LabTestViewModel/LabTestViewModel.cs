using iHealth.Core.Application.ViewModels.Base;
using iHealth.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace iHealth.Core.Application.ViewModels.LabTestViewModel
{
    public class LabTestViewModel : BaseEntityViewModel
    {
        [Required(ErrorMessage = "The name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public virtual ICollection<LabResult>? LabResults { get; set; }
    }
}
