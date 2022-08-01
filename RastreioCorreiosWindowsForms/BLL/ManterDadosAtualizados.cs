using System;
using System.Linq;
using System.Threading.Tasks;
using Correios.Pacotes.Services;
using RastreioCorreiosWindowsForms.DAO;
using RastreioCorreiosWindowsForms.Models;

namespace RastreioCorreiosWindowsForms.BLL
{
    public class ManterDadosAtualizados
    {
        private readonly CrudPacotes crudPacotesDao;
        public ManterDadosAtualizados()
        {
            crudPacotesDao = new CrudPacotes();
        }


        public async Task<Models.CodigosRastreio> RastrearPacoteIndividual(CodigosRastreio objeto)
        {
            try
            {
                Rastreador rastreador = new Rastreador();
                var pacote = await rastreador.ObterPacoteAsync(objeto.CODIGO_RASTREIO);

                string descricaoStatusRastreio = pacote.Historico.FirstOrDefault() != null ? pacote.Historico.FirstOrDefault().Localizacao + " " + pacote.Historico.FirstOrDefault().StatusCorreio : pacote.Observacao;

                await crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, descricaoStatusRastreio);

                objeto.DESCRICAO_GERAL = descricaoStatusRastreio;
                objeto.ULTIMO_PROCESSAMENTO = DateTime.Now;
          

                return objeto;
            }
            catch (Exception ex)
            {
                _ = crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, ex.Message.ToString());
                objeto.DESCRICAO_GERAL = ex.Message;
                return objeto;
            }
        }
    }
}
