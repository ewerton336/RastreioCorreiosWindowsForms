using System;

namespace RastreioCorreiosWindowsForms
{
    public class Status
    {
        public DateTime Data { get; internal set; }
        public string Localizacao { get; internal set; }
        public string StatusCorreio { get; internal set; }
        public string Observacao { get; internal set; }
    }
}
