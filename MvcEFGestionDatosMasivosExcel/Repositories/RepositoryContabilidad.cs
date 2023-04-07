using Microsoft.EntityFrameworkCore;
using MvcEFGestionDatosMasivosExcel.Data;
using MvcEFGestionDatosMasivosExcel.Models;

namespace MvcEFGestionDatosMasivosExcel.Repositories
{
    public class RepositoryContabilidad : IRepoContabilidad
    {
        private ContabilidadContext context;
        public RepositoryContabilidad(ContabilidadContext context)
        {
            this.context = context;
        }

        public async Task DeleteMasiveData(List<Contabilidad> contabilidades)
        {
            await this.context.BulkDeleteAsync(contabilidades);
        }

        public Task<List<Contabilidad>> GetContabilidades()
        {
            return this.context.Contabilidades.ToListAsync();
        }

        public async Task ImportMasiveData(List<Contabilidad> contabilidades)
        {
            await this.context.BulkInsertAsync(contabilidades);
        }

        public async Task UpdateMasiveData(List<Contabilidad> contabilidades, int subirsaldo)
        {
            contabilidades.ForEach(c => c.Saldo += subirsaldo);
            await this.context.BulkUpdateAsync(contabilidades);
        }
    }
}
