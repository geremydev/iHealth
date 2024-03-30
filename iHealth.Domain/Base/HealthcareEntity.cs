using iHealth.Core.Domain.Common;

namespace iHealth.Core.Domain.Base
{
    public class HealthcareEntity : BasePerson
    {
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string ImageURL { get; set; }

    }
}
