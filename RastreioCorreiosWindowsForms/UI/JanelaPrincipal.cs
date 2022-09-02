using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using RastreioCorreiosWindowsForms.DAO;
using DevExpress.XtraGrid.Views.Grid;
using RastreioCorreiosWindowsForms.Models;
using System.Threading;
using DevExpress.LookAndFeel;
using RastreioCorreiosWindowsForms.Properties;

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class JanelaPrincipal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly CrudPacotes crudPacotesDao;
        private readonly BLL.ManterDadosAtualizados manterDadosAtualizados;
        private List<CodigosRastreio> listaAnterior = new List<CodigosRastreio>();
        private bool modoEscuro = false;
        private int intervaloExecucao = 300000;
        public JanelaPrincipal()
        {

            InitializeComponent();
            //aparecer tela de carregando ao iniciar programa
            if (!splashTelaCarregando.IsSplashFormVisible)
            {
                splashTelaCarregando.ShowWaitForm();
                splashTelaCarregando.SetWaitFormDescription("Inicializando Programa...");
            }
            crudPacotesDao = new CrudPacotes();
            manterDadosAtualizados = new BLL.ManterDadosAtualizados();

            Task task = ObterDados();
            //  Task task2 = manterDadosAtualizados.ListarAtualizarPacotes();
            if (splashTelaCarregando.IsSplashFormVisible) splashTelaCarregando.CloseWaitForm();
            #region Manter Worker rodando em outra Thread
            Thread th = new Thread(new ThreadStart(backgroundWorker.RunWorkerAsync));
            th.Start();
            #endregion

            //dicionario de intervalo de execução com tempo em milissegudos
            #region carregar dados na combobox de intervalo de execução
            Dictionary<int, string> ListaIntervaloExecucao = new Dictionary<int, string>
            {
                {1000, "1 segundo"},
                {10000,"10 segundos" },
                {30000, "30 segundos" },
                {60000, "1 minuto" },
                {300000, "5 minutos" },
                {600000, "10 minutos" },
            };


            // atribuir dicionário a lista da combobox de intervalo de execucao
            repositoryItemLookUpEdit1.DataSource = ListaIntervaloExecucao;
            #endregion
        }

        public async Task ObterDados()
        {
            try
            {
                var pacotes = await crudPacotesDao.GetDadosRastreios();
                listaAnterior = pacotes.ToList();
                gridControl.DataSource = listaAnterior;
                int pacotesPendentes = pacotes.Where(p => p.ENTREGUE == false).Count();
                int pacotesEntregues = pacotes.Where(p => p.ENTREGUE == true).Count();
                int pacotesTotal = pacotes.Count();
                barStaticItem1.Caption = $"Pacotes Pendentes: {pacotesPendentes} - Pacotes entregues: {pacotesEntregues} - Total: {pacotesTotal}";
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
        private async void botaoAtualizar_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (!splashTelaCarregando.IsSplashFormVisible)
            {
                splashTelaCarregando.ShowWaitForm();
                splashTelaCarregando.SetWaitFormDescription("Obtendo pacotes do banco de dados.");
            }
            await ObterDados();
            if (splashTelaCarregando.IsSplashFormVisible) splashTelaCarregando.CloseWaitForm();

            gridView.RefreshData();
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

        private async void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //botao atualizar grid
            try
            {
                gridControl.DataSource = listaAnterior;
                gridControl.Refresh();
                gridControl.RefreshDataSource();
                gridView.RefreshData();
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
            if (dadosLinha != null && dadosLinha.DESCRICAO_GERAL != null && dadosLinha.DESCRICAO_GERAL.Contains("aguardando retirada"))
            {
                e.Appearance.BackColor = Color.Salmon;
            }
        }

        private async void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    int numeroLinhas = ((List<CodigosRastreio>)gridView.DataSource).Count();
                    var listaAtualizarBancoDeDados = new List<CodigosRastreio>();

                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        var objeto = (CodigosRastreio)gridView.GetRow(i);
                        if (objeto != null)
                        {
                            if (objeto.ENTREGUE == true) continue;
                            var diferencaMinutos = DateTime.Now.Subtract(objeto.ULTIMO_PROCESSAMENTO);
                            if (diferencaMinutos.TotalMilliseconds < intervaloExecucao) continue;
                            objeto = await RastrearPacote(objeto);

                            // se após rastrear o status for entregue, é adicionado na lista para enviar ao banco de dados
                            if (objeto.ENTREGUE)
                            {
                                if (listaAtualizarBancoDeDados.Count > 10)
                                {
                                    _ = EncerrarPacotes(listaAtualizarBancoDeDados);
                                    listaAtualizarBancoDeDados.Clear();
                                }
                                listaAtualizarBancoDeDados.Add(objeto);
                                // await crudPacotesDao.EncerrarPacoteEntregue(objeto.ID);
                            }
                            var teste = gridView;
                            gridView.RefreshRow(i);
                        }
                        int pacotesPendentes = ((List<CodigosRastreio>)gridView.DataSource).Where(p => p.ENTREGUE == false).Count();
                        int pacotesEntregues = ((List<CodigosRastreio>)gridView.DataSource).Where(p => p.ENTREGUE == true).Count();
                        int pacotesTotal = ((List<CodigosRastreio>)gridView.DataSource).Count();
                        barStaticItem1.Caption = $"Pacotes Pendentes: {pacotesPendentes} - Pacotes entregues: {pacotesEntregues} - Total: {pacotesTotal}";
                    }
                    foreach (var item in listaAtualizarBancoDeDados)
                    {
                        await crudPacotesDao.EncerrarPacoteEntregue(item.ID);
                    }


                    System.Threading.Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "Erro ao tentar executar backGroundWorker");
                }
            }
        }

        private async Task EncerrarPacotes(List<CodigosRastreio> listaAtualizarBancoDeDados)
        {
            foreach (var item in listaAtualizarBancoDeDados)
            {
                await crudPacotesDao.EncerrarPacoteEntregue(item.ID);
            }
        }

        private async Task<CodigosRastreio> RastrearPacote(CodigosRastreio objeto)
        {
            var rastreio = await manterDadosAtualizados.RastrearApi(objeto);
            if (rastreio.DESCRICAO_GERAL.Contains("entregue ao"))
            {
                rastreio.ENTREGUE = true;
            }
            return rastreio;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gridControl.DataSource = listaAnterior;
        }

        private void gridControl_DoubleClick(object sender, EventArgs e)
        {
            var teste = gridView.GetFocusedRow();

        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void botaoMudarTema_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!modoEscuro)
            {
                modoEscuro = true;
                botaoMudarTema.Caption = "Modo claro";
                // botaoMudarTema.ImageOptions.SvgImage = Resources
                UserLookAndFeel.Default.SetSkinStyle(SkinStyle.Office2019Black);
            }
            else
            {
                modoEscuro = false;
                botaoMudarTema.Caption = "Modo escuro";
                UserLookAndFeel.Default.SetSkinStyle(SkinStyle.Office2019Colorful);
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            // comando ao ativar 2 vezes em um rastreio dentro do 
        }

        private void lookupIntervaloExecucao_EditValueChanged(object sender, EventArgs e)
        {
            intervaloExecucao = (int)lookupIntervaloExecucao.EditValue;

        }
    }
}

