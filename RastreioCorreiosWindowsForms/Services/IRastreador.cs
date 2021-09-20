using RastreioCorreiosWindowsForms.Models;
using System.Threading.Tasks;

namespace RastreioCorreiosWindowsForms.Services
{
    public interface IRastreador
    {
        Pacote ObterPacote(string codigo);
        Task<Pacote> ObterPacoteAsync(string codigo);
    }
}
