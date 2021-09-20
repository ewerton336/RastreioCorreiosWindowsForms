using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RastreioCorreiosWindowsForms
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var teste = new BLL.ManterDadosAtualizados();
            Task.Run(teste.ListarAtualizarPacotes);
            Application.Run(new UI.JanelaPrincipal());
        }
    }
}
