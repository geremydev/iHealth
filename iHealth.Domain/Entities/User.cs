using iHealth.Core.Domain.Common;

namespace iHealth.Core.Domain.Entities
{
    public class User : BasePerson
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin,
        Secretary
    }
}
