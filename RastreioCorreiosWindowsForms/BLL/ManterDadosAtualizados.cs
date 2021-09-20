using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<int> ListarAtualizarPacotes()
        {
                var pacotes = crudPacotesDao.GetDadosRastreios();
                foreach (var objeto in pacotes)
                {
                var diferncaTempoUltimaAtualizacao = objeto.ULTIMO_PROCESSAMENTO.Subtract(DateTime.Now);

                if (diferncaTempoUltimaAtualizacao.Minutes < 5)
                {
                    continue;
                }
                await RastrearPacotes(objeto);
                }

                return pacotes.Count();
        }


        public async Task RastrearPacotes(CodigosRastreio objeto)
        {

            try
            {
                var result = new Correios.NET.Services().GetPackageTrackingAsync(objeto.CODIGO_RASTREIO).Result;
                crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, result.LastStatus.ToString());
                if (result.IsDelivered)
                {
                    await crudPacotesDao.EncerrarPacoteEntregue(objeto.ID);
                }
            }
            catch (Exception ex)
            {
                crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, ex.InnerException.Message.ToString());
            }
        }
    }



}
