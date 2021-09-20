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

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class JanelaPrincipal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly CrudPacotes crudPacotesDao;
        private readonly BLL.ManterDadosAtualizados manterDadosAtualizados;
        public JanelaPrincipal()
        {
            crudPacotesDao = new CrudPacotes(RastreioCorreiosWindowsForms.Helper.DBConnectionOracle);
            manterDadosAtualizados = new BLL.ManterDadosAtualizados();
            InitializeComponent();
            ObterDados();
        }

        public void ObterDados()
        {
            var result = crudPacotesDao.GetDadosRastreios();
            gridControl.DataSource = result;
            bsiRecordsCount.Caption = "Registros : " + result.Count();
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

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ObterDados();
            Detalhes.ToString();
        }

        private void bbiDelete_ItemClick(object sender, EventArgs e)
        {
            GridView gridView = gridControl.FocusedView as GridView;
            var dadosLinhaSelecionada =(Models.CodigosRastreio) gridView.GetRow(gridView.FocusedRowHandle);
            crudPacotesDao.DeletarPacote(dadosLinhaSelecionada.ID);
            ObterDados();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("Iniciando atualização dos pacotes.");
            Task.Run(manterDadosAtualizados.ListarAtualizarPacotes);
            // manterDadosAtualizados.ListarAtualizarPacotes();
            ObterDados();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            var cadastroEmMassa = new CadastroPacoteEmMassa();
            cadastroEmMassa.Show();
            ObterDados();
        }

      
    }
}