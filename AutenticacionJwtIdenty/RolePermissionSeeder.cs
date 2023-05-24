using AutenticacionJwtIdenty.Context;
using AutenticacionJwtIdenty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutenticacionJwtIdenty
{
    public class RolePermissionSeeder
    {
        public static async Task SeedRolesAndPermissions(BdContext context)
        {
            if (!context.Roles.Any())
            {
                //Crear roles
                var roles = new List<Role>
                {
                    new Role { Name = "Admin", NormalizedName = "Admin" },
                    new Role { Name = "User", NormalizedName = "User" }
                };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            if (!context.Permissions.Any())
            {
                //Crear permisos
                var permissions = new List<Permission>
                {
                    new Permission { Nombre = "CrearUsuario" },
                    new Permission { Nombre = "EditarUsuario" },
                    new Permission { Nombre = "EliminarUsuario" }
                };
                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();

                var roleFirst = await context.Roles.FirstAsync();

                //Asignar permisos a roles
                var rolePermissions = new List<RolePermission>
                {
                    new RolePermission { RoleId = roleFirst.Id, PermissionId = permissions[0].Id },
                    new RolePermission { RoleId = roleFirst.Id, PermissionId = permissions[1].Id },
                    new RolePermission { RoleId = roleFirst.Id, PermissionId = permissions[2].Id }
                };
                context.RolePermissions.AddRange(rolePermissions);
                await context.SaveChangesAsync();
            }
        }
    }
}
