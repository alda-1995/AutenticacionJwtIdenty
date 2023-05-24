using System.ComponentModel.DataAnnotations.Schema;

namespace AutenticacionJwtIdenty.Models
{
    public class Permission
    {
        public Permission()
        {
            Id = Guid.NewGuid().ToString();
        }

        public  string Id { get; set; }
        public string Nombre { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
    }
}
