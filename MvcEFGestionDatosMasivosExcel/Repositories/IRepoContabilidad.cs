using MvcEFGestionDatosMasivosExcel.Models;

namespace MvcEFGestionDatosMasivosExcel.Repositories
{
    public interface IRepoContabilidad
    {
        public Task<List<Contabilidad>> GetContabilidades();
        public Task ImportMasiveData(List<Contabilidad> contabilidades);
        public Task UpdateMasiveData(List<Contabilidad> contabilidades, int subirsaldo);
        public Task DeleteMasiveData(List<Contabilidad> contabilidades);
    }
}
