using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using RastreioCorreiosWindowsForms.Models;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RastreioCorreiosWindowsForms.DAO
{
    public class CrudPacotes : DaoConexao
    {
        public CrudPacotes()
        {
            
        }

        public async Task < IEnumerable<CodigosRastreio>> GetDadosRastreios()
        {
            try
            {
                var sql = @"SELECT
                        ID
                        ,CODIGO_RASTREIO
                        ,DESCRICAO_GERAL
                        ,ULTIMO_PROCESSAMENTO
                        ,CONTEUDO_PACOTE
                        ,PACOTE_DOS_CLIENTES
                        ,ENTREGUE
                        FROM CORREIOS_RASTREAMENTO
                        ORDER BY ENTREGUE ASC
                        ,ID DESC";
                var result = await DbConnection.QueryAsync<CodigosRastreio>(sql);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<int> InserirPacote(string codigoRastreio, int clienteCheck, string conteudoPacote)
        {
            try
            {
                var cadastrado = await VerificarPacoteJaCadastrado(codigoRastreio);
                if (cadastrado < 1)
                {
                    string SQL = @"   INSERT INTO CORREIOS_RASTREAMENTO
                                    ( CODIGO_RASTREIO
                                      ,ENTREGUE
                                      ,PACOTE_DOS_CLIENTES
                                      ,CONTEUDO_PACOTE)
                                    VALUES(@RASTREIO,  0, @CLIENTE_CHECK, @CONTEUDOPACOTE)";

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


        public async Task<int> InserirVariosPacotes(List<string> rastreios, int clienteCheck, string conteudoPacote)
        {
            try
            {
                string SQL = @"   INSERT INTO CORREIOS_RASTREAMENTO
                                    ( CODIGO_RASTREIO
                                      ,ENTREGUE
                                      ,PACOTE_DOS_CLIENTES
                                      ,CONTEUDO_PACOTE)
                                        VALUES";


                foreach (var item in rastreios)
                {
                    SQL += $", ({item}, 0, {clienteCheck}, {conteudoPacote})";
                }

                    var result = await DbConnection.ExecuteAsync(SQL, new { RASTREIOS = rastreios, CLIENTE_CHECK = clienteCheck, CONTEUDOPACOTE = conteudoPacote });
                    return result;
               
               

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task EncerrarPacoteEntregue(int idPacote)
        {
            string sql = @"UPDATE CORREIOS_RASTREAMENTO
                            SET ENTREGUE = 1
                            ,DATA_ENCERRAMENTO = SYSDATE()- interval '3' hour
                            WHERE ID = @ID";
             await DbConnection.ExecuteAsync(sql, new { ID = idPacote });
             DbConnection.Close();
        }

        public async Task AtualizarDescricaoRastreio(string codRastreio, string descricao)
        {
            string sql = @"UPDATE CORREIOS_RASTREAMENTO
                            SET DESCRICAO_GERAL = @DESCRICAO
                            ,ULTIMO_PROCESSAMENTO = SYSDATE()- interval '3' hour
                            WHERE CODIGO_RASTREIO = @CODIGO";
            var result = await DbConnection.ExecuteAsync(sql, new { DESCRICAO = descricao, CODIGO = codRastreio });
        }

        public void DeletarPacote (int id)
        {
            string sql = @"DELETE FROM CORREIOS_RASTREAMENTO
                        WHERE ID = @ID";
            DbConnection.Execute(sql, new { ID = id });
        }

        public async Task<int> VerificarPacoteJaCadastrado(string codrastreio)
        {
            var sql = @"SELECT CODIGO_RASTREIO Codigo
                        FROM CORREIOS_RASTREAMENTO
                        WHERE CODIGO_RASTREIO = @CODIGO";
            var result = await DbConnection.QueryAsync(sql, new {CODIGO = codrastreio });
            int contadorResultados = result.Count();
            return contadorResultados;
        }

        public async Task<IEnumerable<string>> VerificarVariosPacotesCadastrados(List<string> CodigosRastreio)
        {
            var sql = @"SELECT CODIGO_RASTREIO Codigo
                        FROM CORREIOS_RASTREAMENTO
                        WHERE CODIGO_RASTREIO IN @CODIGO";
            return await DbConnection.QueryAsync<string>(sql, new { CODIGO = CodigosRastreio });
        }
    }
}
