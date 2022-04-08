using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using RastreioCorreiosWindowsForms.Models;
using System.Threading.Tasks;

namespace RastreioCorreiosWindowsForms.DAO
{
    public class CrudPacotes : DaoConexao
    {

        public CrudPacotes(IDbConnection dbConnection) : base(dbConnection)
        {

        }


        public async Task < IEnumerable<CodigosRastreio>> GetDadosRastreios()
        {
            var sql = @"SELECT
                        ID
                        ,CODIGO_RASTREIO
                        ,DESCRICAO_GERAL
                        ,ULTIMO_PROCESSAMENTO
                        ,CONTEUDO_PACOTE
                        ,PACOTE_DOS_CLIENTES
                        ,ENTREGUE
                        FROM CORREIOS.RASTREAMENTO_CORREIOS
                        --WHERE ENTREGUE = 0
                        ORDER BY ID DESC";
            var result = await DbConnection.QueryAsync<CodigosRastreio>(sql);
            return result;
        }

        public async Task<int> InserirPacote(string codigoRastreio, int clienteCheck, string conteudoPacote)
        {
            try
            {
                var cadastrado = await VerificarPacoteJaCadastrado(codigoRastreio);
                if (cadastrado < 1)
                {
                    string SQL = @"   INSERT INTO CORREIOS.RASTREAMENTO_CORREIOS
                                    ( CODIGO_RASTREIO
                                      ,ENTREGUE
                                      ,PACOTE_DOS_CLIENTES
                                      ,CONTEUDO_PACOTE)
                                    VALUES(:RASTREIO,  0, :CLIENTE_CHECK, :CONTEUDOPACOTE)";

                    var result = await DbConnection.ExecuteAsync(SQL, new { RASTREIO = codigoRastreio, CLIENTE_CHECK = clienteCheck, CONTEUDOPACOTE = conteudoPacote });
                    return result;
                }
               //caso ja tenha pacote cadastrado ele nao faz nada
                return 0;
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task EncerrarPacoteEntregue(int idPacote)
        {
            string sql = @"UPDATE CORREIOS.RASTREAMENTO_CORREIOS
                            SET ENTREGUE = 1
                            ,DATA_ENCERRAMENTO = SYSDATE
                            WHERE ID = :ID";
            var result = await DbConnection.ExecuteAsync(sql, new { ID = idPacote });
        }

        public async Task AtualizarDescricaoRastreio(string codRastreio, string descricao)
        {
            string sql = @"UPDATE CORREIOS.RASTREAMENTO_CORREIOS
                            SET DESCRICAO_GERAL = :DESCRICAO
                            ,ULTIMO_PROCESSAMENTO = SYSDATE
                            WHERE CODIGO_RASTREIO = :CODIGO";
            var result = await DbConnection.ExecuteAsync(sql, new { DESCRICAO = descricao, CODIGO = codRastreio });
        }



        public void DeletarPacote (int id)
        {
            string sql = @"DELETE FROM CORREIOS.RASTREAMENTO_CORREIOS
                        WHERE ID = :ID";
            DbConnection.Execute(sql, new { ID = id });
        }


        public async Task<IEnumerable<Pacote>> GetCodigosRastreios()
        {
            var sql = @"SELECT CODIGO_RASTREIO Codigo
                        FROM CORREIOS.RASTREAMENTO_CORREIOS 
                        WHERE ENTREGUE != 1";
            var result = await DbConnection.QueryAsync<Pacote>(sql);
            return result;
        }

        public async Task<int> VerificarPacoteJaCadastrado(string codrastreio)
        {
            var sql = @"SELECT CODIGO_RASTREIO Codigo
                        FROM CORREIOS.RASTREAMENTO_CORREIOS
                        WHERE CODIGO_RASTREIO = :CODIGO";
            var result = await DbConnection.QueryAsync(sql, new {CODIGO = codrastreio });
            int contadorResultados = result.Count();
            return contadorResultados;
        }



     /*   public void ObterUltima()
        {
            while (true)
            {
                var listaPacotes = GetCodigosRastreios();
                Rastreador rastreador = new Rastreador();

                foreach (Pacote codigoRastreio in listaPacotes)
                {
                    Pacote pacote = rastreador.ObterPacote(codigoRastreio.Codigo);

                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(DateTime.Now.ToString() + $" @@@@@@ ÚLTIMA ATUALIZAÇÃO DO PACOTE {codigoRastreio.Codigo} @@@@@@@ \n ");
                    Console.ResetColor();
                    Console.WriteLine("Data - Localização - StatusCorreio - Observação");

                    if (pacote.Historico.Count > 0 && (pacote.Historico[0].StatusCorreio.Contains("saiu") || pacote.Historico[0].StatusCorreio.Contains("retirada")))
                    {

                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($@" {pacote.Historico[0].Data} {pacote.Historico[0].Localizacao} {pacote.Historico[0].StatusCorreio} {pacote.Historico[0].Observacao}" + "\n\n");
                        Console.ResetColor();
                    }
                    else if (pacote.Historico.Count > 0 && !pacote.Historico[0].StatusCorreio.Contains("entregue ao destinatário"))
                    {
                        Console.WriteLine($@" {pacote.Historico[0].Data} {pacote.Historico[0].Localizacao} {pacote.Historico[0].StatusCorreio} {pacote.Historico[0].Observacao}" + "\n\n");
                    }
                    if (pacote.Historico.Count > 0 && (pacote.Historico[0].StatusCorreio.Contains("entregue ao destinatário")))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($@" {pacote.Historico[0].Data} {pacote.Historico[0].Localizacao} {pacote.Historico[0].StatusCorreio} {pacote.Historico[0].Observacao}" + "\n\n");
                        Console.WriteLine("Pedido Entregue. Encerrando pendência de rastreamento.");
                        Console.ResetColor();
                        crudPacotes.EncerrarPacoteEntregue(codigoRastreio.ID);
                    }


                }
                Thread.Sleep(TimeSpan.FromSeconds(300));
            }
        } */

    }
}
