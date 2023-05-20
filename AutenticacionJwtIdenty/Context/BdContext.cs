using AutenticacionJwtIdenty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutenticacionJwtIdenty.Context
{
    public class BdContext : IdentityDbContext<IdentityUser>
    {
        public BdContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Grupo> Grupos { get; set; }
    }
}
