﻿using System;
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

        public async Task ListarAtualizarPacotes()
        {
            while (true)
            {
                var pacotes = crudPacotesDao.GetDadosRastreios();

                foreach (var objeto in pacotes)
                {
                    var diferncaTempoUltimaAtualizacao = DateTime.Now.Subtract(objeto.ULTIMO_PROCESSAMENTO);

                    if (diferncaTempoUltimaAtualizacao.TotalMinutes < 5)
                    {
                        continue;
                    }
                    else
                    {
                        await RastrearPacotes(objeto);
                    }
                }
                Task.Delay(60000);
            }

           
        }


        public async Task RastrearPacotes(CodigosRastreio objeto)
        {

            try
            {
                var result = new Correios.NET.Services().GetPackageTrackingAsync(objeto.CODIGO_RASTREIO).Result;
                crudPacotesDao.AtualizarDescricaoRastreio(objeto.CODIGO_RASTREIO, (result.LastStatus.ToString()));
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
