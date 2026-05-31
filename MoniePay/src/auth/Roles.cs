namespace MoniePay.src.auth
{
    public class Roles
    {
        public Roles() { }
        public Roles(RoleType roleName)
        {
            this.RoleName = roleName;
            this.RoleId = Guid.NewGuid();
        }

        public Guid RoleId { get; set; }
        public RoleType RoleName { get; set; }

        [Flags]
        public enum RoleType
        {
            None = 0,
            Admin = 1 << 0, // 1
            Customer = 1 << 1, // 2
            Merchant = 1 << 2, // 4
            Audit = 1 << 3, // 8
            Finance = 1 << 4, // 16
            Support = 1 << 5, // 32
            CustomerRep = 1 << 6  // 64
        }
    }
}
