using System.ComponentModel.DataAnnotations.Schema;

namespace iHealth.Core.Domain.Common
{
    [NotMapped]
    public class BasePerson : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
    }
}
