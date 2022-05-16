using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RastreioCorreiosWindowsForms.DAO;
using RastreioCorreiosWindowsForms.BLL;

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class CadastroPacoteEmMassa : DevExpress.XtraEditors.XtraForm
    {
        private readonly ManterDadosAtualizados manterDadosAtualizados;
        private readonly CrudPacotes crudPacotes;
        public CadastroPacoteEmMassa()
        {
            crudPacotes = new CrudPacotes(RastreioCorreiosWindowsForms.Helper.DBConnectionSql);
            manterDadosAtualizados = new ManterDadosAtualizados();
            InitializeComponent();
        }

        private void botaoCadastrar_Click(object sender, EventArgs e)
        {
             int clienteCheck = checkPacoteClientes.Checked ? 1 : 0;
              string conteudoPacote = textoDescricao.Text;
            var conteudoCaixaTexto = caixaTexto.Text;
            var rastreios = conteudoCaixaTexto.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
           
            foreach (string rastreio in rastreios)
            {
                _ =crudPacotes.InserirPacote(rastreio, clienteCheck, conteudoPacote);
            }
            Close();
            Task.Run(manterDadosAtualizados.ListarAtualizarPacotes);
        }
    }
}