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

namespace RastreioCorreiosWindowsForms.UI
{
    public partial class JanelinhaCadastroPacote : DevExpress.XtraEditors.XtraForm
    {
        public JanelinhaCadastroPacote()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var dados = textBox1.Text;
                string conteudoPacote = textBox2.Text;
                int clienteCheck = checkCliente.Checked ? 1 : 0;
                var cadastro = new DAO.CrudPacotes(RastreioCorreiosWindowsForms.Helper.DBConnectionSql).InserirPacote(dados, clienteCheck, conteudoPacote);
                if (cadastro.Result == 0)
                {
                    XtraMessageBox.Show("O pacote não foi cadastrado pois já existe um idêntico no banco de dados.");
                }
                Close();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.ToString());
            }
        }

        
    }
}