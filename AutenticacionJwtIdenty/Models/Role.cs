using Microsoft.AspNetCore.Identity;

namespace AutenticacionJwtIdenty.Models
{
    public class Role : IdentityRole
    {
        public Role()
        {
            RolePermissions = new List<RolePermission>();
        }
        public List<RolePermission> RolePermissions { get; set; }
    }
}
