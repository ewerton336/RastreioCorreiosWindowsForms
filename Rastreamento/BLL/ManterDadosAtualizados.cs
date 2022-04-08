using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            crudPacotesDao = new CrudPacotes(Helper.DBConnectionOracle);
        }

        public async Task ListarAtualizarPacotes()
        {
            while (true)
            {
                var pacotes = crudPacotesDao.GetDadosRastreios().Result;

                foreach (var objeto in pacotes)
                {
                    if (objeto.ENTREGUE == true) continue;
                    var teste = DateTime.Now.Subtract(objeto.ULTIMO_PROCESSAMENTO);
                    if (teste.TotalMinutes < 3) continue;

                    //if (objeto.DESCRICAO_GERAL != null && objeto.DESCRICAO_GERAL.Contains("entregue ao")) continue;
                    await RastrearPacotes(objeto);
                }
                await Task.Delay(60000);
            }
        }

        public async Task RastrearPacotes(CodigosRastreio objeto)
        {
            try
            {
                Rastreador rastreador = new Rastreador();
                var pacote =  await rastreador.ObterPacoteAsync(objeto.CODIGO_RASTREIO);

                string descricaoStatusRastreio = pacote.Historico.FirstOrDefault() != null ? pacote.Historico.FirstOrDefault().Localizacao + " " + pacote.Historico.FirstOrDefault().StatusCorreio : pacote.Observacao;

                await crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, descricaoStatusRastreio);  
            }
            catch (Exception ex)
            {
                await crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, ex.Message.ToString());
            }
        }
    }
}
