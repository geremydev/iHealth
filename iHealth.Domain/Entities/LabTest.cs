using iHealth.Core.Domain.Common;
namespace iHealth.Core.Domain.Entities
{
    public class LabTest : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<LabResult>? LabResults { get; set; }

    }
}
