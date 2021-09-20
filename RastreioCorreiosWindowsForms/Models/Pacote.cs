using System.Collections.Generic;

namespace RastreioCorreiosWindowsForms
{
    public class Pacote
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public List<Status> Historico { get; internal set; }
        public bool Entregue { get; internal set; }
        public bool IsValid { get; internal set; }
        public string Observacao { get; internal set; }
    }
}
