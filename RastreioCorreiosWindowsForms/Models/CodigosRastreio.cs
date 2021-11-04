using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioCorreiosWindowsForms.Models
{
    public class CodigosRastreio
    {
        public int ID { get; set; }
        public string CODIGO_RASTREIO { get; set; }
        public string DESCRICAO_GERAL { get; set; }
        public bool ENTREGUE { get; set; }
        public DateTime ULTIMO_PROCESSAMENTO { get; set; }
        public string CONTEUDO_PACOTE { get; set; }
        public bool PACOTE_DOS_CLIENTES { get; set; }
    }
}
