using Microsoft.EntityFrameworkCore;
using MvcEFGestionDatosMasivosExcel.Models;

namespace MvcEFGestionDatosMasivosExcel.Data
{
    public class ContabilidadContext: DbContext
    {
        public ContabilidadContext(DbContextOptions<ContabilidadContext> options) : base(options) { }
        public DbSet<Contabilidad> Contabilidades { get;set; }
    }
}
