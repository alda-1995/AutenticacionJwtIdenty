using System.ComponentModel.DataAnnotations;

namespace AutenticacionJwtIdenty.Models
{
    public class Grupo
    {
        [Key]
        public int GrupoId { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
