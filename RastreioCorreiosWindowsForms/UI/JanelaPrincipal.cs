using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RastreioCorreiosWindowsForms.DAO;
using DevExpress.XtraGrid.Views.Grid;
using RastreioCorreiosWindowsForms.Models;

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class JanelaPrincipal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly CrudPacotes crudPacotesDao;
        private readonly BLL.ManterDadosAtualizados manterDadosAtualizados;
        public JanelaPrincipal()
        {
            InitializeComponent();
            //aparecer tela de carregando ao iniciar programa
            if (!splashTelaCarregando.IsSplashFormVisible)
            {
                splashTelaCarregando.ShowWaitForm();
                splashTelaCarregando.SetWaitFormDescription("Inicializando Programa...");
            }
            crudPacotesDao = new CrudPacotes(RastreioCorreiosWindowsForms.Helper.DBConnectionSql);
            manterDadosAtualizados = new BLL.ManterDadosAtualizados();

            Task task = ObterDados();
            Task task2 = manterDadosAtualizados.ListarAtualizarPacotes();
            if (splashTelaCarregando.IsSplashFormVisible) splashTelaCarregando.CloseWaitForm();
        }

        public async Task ObterDados()
        {
            try
            {
                var pacotes = await crudPacotesDao.GetDadosRastreios();
                gridControl.DataSource = pacotes;

                foreach (var pacote in pacotes)
                {
                    if (pacote.ENTREGUE == false && pacote.DESCRICAO_GERAL != null && pacote.DESCRICAO_GERAL.Contains("entregue ao"))
                    {
                        await crudPacotesDao.EncerrarPacoteEntregue(pacote.ID);
                    }
                }

                barStaticItem1.Caption = $"Pacotes: {pacotes.Count()}";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "erro ao tentar obter dados");
            }
        }



        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            var janelinha = new JanelinhaCadastroPacote();
            janelinha.Show();
        }

        //botao atualizar
        private void botaoAtualizar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!splashTelaCarregando.IsSplashFormVisible)
            {
                splashTelaCarregando.ShowWaitForm();
                splashTelaCarregando.SetWaitFormDescription("Obtendo pacotes do banco de dados.");
            }
            if (!backgroundWorker.IsBusy)backgroundWorker.RunWorkerAsync();
            //_ = ObterDados();
            if (splashTelaCarregando.IsSplashFormVisible) splashTelaCarregando.CloseWaitForm();
           gridControl.RefreshDataSource();
        }

        private void bbiDelete_ItemClick(object sender, EventArgs e)
        {
            try
            {
                GridView gridView = gridControl.FocusedView as GridView;
                var dadosLinhaSelecionada = (Models.CodigosRastreio)gridView.GetRow(gridView.FocusedRowHandle);
                crudPacotesDao.DeletarPacote(dadosLinhaSelecionada.ID);
                _ = ObterDados();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "erro ao tentar deletar"); ;
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                XtraMessageBox.Show("Iniciando atualização dos pacotes.");
                Task.Run(manterDadosAtualizados.ListarAtualizarPacotes);
                // manterDadosAtualizados.ListarAtualizarPacotes();
                ObterDados();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "erro ao tentar atualizar pacotes"); ;
            }
        }

        private async void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var cadastroEmMassa = new CadastroPacoteEmMassa();
                cadastroEmMassa.ShowDialog();
                await ObterDados();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "erro ao tentar cadastrar em massa"); ;
            }
        }

        private void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            var view = sender as GridView;
            var dadosLinha = (CodigosRastreio)view.GetRow(e.RowHandle);
            if (dadosLinha.DESCRICAO_GERAL != null && dadosLinha.DESCRICAO_GERAL.Contains("aguardando retirada"))
            {
                e.Appearance.BackColor = Color.Salmon;
            }
        }

        private async void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var listaAnterior = (List<CodigosRastreio>)gridControl.DataSource;
            foreach (var objeto in listaAnterior)
            {
                if (objeto.ENTREGUE == true) continue;
                var teste = DateTime.Now.Subtract(objeto.ULTIMO_PROCESSAMENTO);
                if (teste.TotalMinutes < 3) continue;

                //if (objeto.DESCRICAO_GERAL != null && objeto.DESCRICAO_GERAL.Contains("entregue ao")) continue;
                var rastreio = await manterDadosAtualizados.AtualizarPacotesDiretamente(objeto);
                objeto.DESCRICAO_GERAL = rastreio.DESCRICAO_GERAL;
                objeto.ULTIMO_PROCESSAMENTO = rastreio.ULTIMO_PROCESSAMENTO;
                objeto.ENTREGUE = rastreio.ENTREGUE;
                //atualização constante da tela
                // gridControl.RefreshDataSource();
            }
            e.Result = listaAnterior;
            barStaticItem1.Caption = $"Pacotes: {listaAnterior.Count()}";
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gridControl.DataSource = e.Result;  
        }

        private void gridControl_DoubleClick(object sender, EventArgs e)
        {
            var teste = gridView.GetFocusedRow();


        }
    }
}