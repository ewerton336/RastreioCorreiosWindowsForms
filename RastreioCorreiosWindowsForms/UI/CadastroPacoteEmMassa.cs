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
            crudPacotes = new CrudPacotes(RastreioCorreiosWindowsForms.Helper.DBConnectionSql);
            manterDadosAtualizados = new ManterDadosAtualizados();
            InitializeComponent();
        }

        private async void botaoCadastrar_Click(object sender, EventArgs e)
        {
            if (!splashTelaCarregando.IsSplashFormVisible)
            {
                splashTelaCarregando.ShowWaitForm();
            }
             int clienteCheck = checkPacoteClientes.Checked ? 1 : 0;
              string conteudoPacote = textoDescricao.Text;
            var conteudoCaixaTexto = caixaTexto.Text;
            var rastreios = conteudoCaixaTexto.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            int itemAtual = 1;

            //cadastro em massa dos pacotes
            Parallel.ForEach(rastreios, rastreio =>
            {
                splashTelaCarregando.SetWaitFormDescription($"Salvando {itemAtual} de {rastreios.Count()} ");
                _ = crudPacotes.InserirPacote(rastreio, clienteCheck, conteudoPacote);
                itemAtual++;
            });

            /*foreach (string rastreio in rastreios)
            {
                splashTelaCarregando.SetWaitFormDescription($"Salvando {itemAtual} de {rastreios.Count()} ");   
                await crudPacotes.InserirPacote(rastreio, clienteCheck, conteudoPacote);
                itemAtual++;
            }
            */
           if (splashTelaCarregando.IsSplashFormVisible)
            {
                splashTelaCarregando.CloseWaitForm();
            }
            Close();
        }
    }
}