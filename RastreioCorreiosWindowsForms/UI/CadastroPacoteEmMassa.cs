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
            crudPacotes = new CrudPacotes(RastreioCorreiosWindowsForms.Helper.DBConnectionOracle);
            manterDadosAtualizados = new ManterDadosAtualizados();
            InitializeComponent();
        }

        private void botaoCadastrar_Click(object sender, EventArgs e)
        {
            var conteudoCaixaTexto = caixaTexto.Text;
            var rastreios = conteudoCaixaTexto.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int clienteCheck = checkPacoteClientes.Checked ? 1 : 0;
            foreach (string rastreio in rastreios)
            {
                crudPacotes.InserirPacote(rastreio, clienteCheck, "");
                Close();
                Task.Run(manterDadosAtualizados.ListarAtualizarPacotes);
            }
        }
    }
}