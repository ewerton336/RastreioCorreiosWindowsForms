﻿using DevExpress.XtraBars;
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
using DevExpress.XtraGrid.Columns;

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class JanelaPrincipal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly CrudPacotes crudPacotesDao;
        private readonly BLL.ManterDadosAtualizados manterDadosAtualizados;
        private List<CodigosRastreio> listaAnterior = new List<CodigosRastreio>();
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
            backgroundWorker.RunWorkerAsync();
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
            if (!backgroundWorker.IsBusy)backgroundWorker.RunWorkerAsync();
           
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

                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        var objeto = (CodigosRastreio)gridView.GetRow(i);
                        if (objeto != null)
                        {
                            if (objeto.ENTREGUE == true) continue;
                            var diferencaMinutos = DateTime.Now.Subtract(objeto.ULTIMO_PROCESSAMENTO);
                            if (diferencaMinutos.TotalMinutes < 3) continue;

                            //if (objeto.DESCRICAO_GERAL != null && objeto.DESCRICAO_GERAL.Contains("entregue ao")) continue;
                            var rastreio = await manterDadosAtualizados.RastrearPacoteIndividual(objeto);
                            if (rastreio.DESCRICAO_GERAL.Contains("entregue ao"))
                            {
                                await crudPacotesDao.EncerrarPacoteEntregue(rastreio.ID);
                                rastreio.ENTREGUE = true;
                            }
                            objeto = rastreio;
                            gridView.RefreshRow(i);
                        }
                    }
                    barStaticItem1.Caption = $"Pacotes: {numeroLinhas}";
                    System.Threading.Thread.Sleep(10000);
                }
                catch (Exception ex)
                {

                    XtraMessageBox.Show(ex.Message, "Erro ao tentar executar backGroundWorker");
                } 
            }
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
    }
}