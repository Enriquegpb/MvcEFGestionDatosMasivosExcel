using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcEFGestionDatosMasivosExcel.Models
{
    [Table("CONTABILIDAD")]
    public class Contabilidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        [Column("ID")]
        public int Id { get; set; }
        [Column("APELLIDOS")]
        public string Apellidos { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("DIRECCION")]
        public string Direccion { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("SALDO")]
        public int Saldo { get; set; }
    }
}
