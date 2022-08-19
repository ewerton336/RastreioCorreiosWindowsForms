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
using RastreioCorreiosWindowsForms.UI;

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class CadastroPacoteEmMassa : DevExpress.XtraEditors.XtraForm
    {
        private readonly ManterDadosAtualizados manterDadosAtualizados;
        private readonly CrudPacotes crudPacotes;
        public CadastroPacoteEmMassa()
        {
            crudPacotes = new CrudPacotes();
            manterDadosAtualizados = new ManterDadosAtualizados();
            InitializeComponent();
        }

        private async void botaoCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!splashTelaCarregando.IsSplashFormVisible)
                {
                    splashTelaCarregando.ShowWaitForm();
                }
                int clienteCheck = checkPacoteClientes.Checked ? 1 : 0;
                string conteudoPacote = textoDescricao.Text;
                var conteudoCaixaTexto = caixaTexto.Text;
                var rastreios = (conteudoCaixaTexto.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).ToList();


                var pacotesJaCadstrados = await crudPacotes.VerificarVariosPacotesCadastrados(rastreios);

                var pacotesACadastrar = rastreios.Except(pacotesJaCadstrados).ToList();


                if (pacotesACadastrar.Count() > 0 ) await crudPacotes.InserirVariosPacotes(pacotesACadastrar, clienteCheck, conteudoPacote);
               
                

                if (splashTelaCarregando.IsSplashFormVisible)
                {
                    splashTelaCarregando.CloseWaitForm();
                }
                Close();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Erro ao tentar cadastrar pacotes em massa");
            }
        }
    }
}