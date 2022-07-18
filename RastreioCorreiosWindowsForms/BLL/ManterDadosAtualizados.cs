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
            crudPacotesDao = new CrudPacotes(Helper.DBConnectionSql);
        }

        public async Task ListarAtualizarPacotes()
        {
            while (true)
            {
                var pacotes = await crudPacotesDao.GetDadosRastreios();

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

        public async Task<CodigosRastreio> AtualizarPacotesDiretamente(CodigosRastreio codigoRastreio)
        {
            await RastrearPacoteIndividual(codigoRastreio);
            return codigoRastreio;
        }

        public async Task<Models.CodigosRastreio> RastrearPacoteIndividual(CodigosRastreio objeto)
        {
            try
            {
                Rastreador rastreador = new Rastreador();
                var pacote = await rastreador.ObterPacoteAsync(objeto.CODIGO_RASTREIO);

                string descricaoStatusRastreio = pacote.Historico.FirstOrDefault() != null ? pacote.Historico.FirstOrDefault().Localizacao + " " + pacote.Historico.FirstOrDefault().StatusCorreio : pacote.Observacao;

                _ = crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, descricaoStatusRastreio);

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

        public async Task RastrearPacotes(CodigosRastreio objeto)
        {
            try
            {
                Rastreador rastreador = new Rastreador();
                var pacote = await rastreador.ObterPacoteAsync(objeto.CODIGO_RASTREIO);

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
