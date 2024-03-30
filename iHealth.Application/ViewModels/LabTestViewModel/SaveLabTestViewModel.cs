using iHealth.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.LabTestViewModel
{
    public class SaveLabTestViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public virtual ICollection<LabResult>? LabResults { get; set; }
    }
}
