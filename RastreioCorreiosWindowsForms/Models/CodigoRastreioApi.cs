using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioCorreiosWindowsForms.Models
{
    public class CodigoRastreioApi
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Endereco
        {
            public string cidade { get; set; }
            public string uf { get; set; }
        }

        public class Evento
        {
            public string codigo { get; set; }
            public string descricao { get; set; }
            public DateTime dtHrCriado { get; set; }
            public string tipo { get; set; }
            public Unidade unidade { get; set; }
            public string urlIcone { get; set; }
        }

        public class Objeto
        {
            public string codObjeto { get; set; }
            public List<Evento> eventos { get; set; }
            public string modalidade { get; set; }
            public TipoPostal tipoPostal { get; set; }
            public bool habilitaAutoDeclaracao { get; set; }
            public bool permiteEncargoImportacao { get; set; }
            public bool habilitaPercorridaCarteiro { get; set; }
            public bool bloqueioObjeto { get; set; }
            public bool possuiLocker { get; set; }
            public bool habilitaLocker { get; set; }
            public bool habilitaCrowdshipping { get; set; }
        }

        public class Root
        {
            public List<Objeto> objetos { get; set; }
            public int quantidade { get; set; }
            public string resultado { get; set; }
            public string versao { get; set; }
        }

        public class TipoPostal
        {
            public string categoria { get; set; }
            public string descricao { get; set; }
            public string sigla { get; set; }
        }

        public class Unidade
        {
            public Endereco endereco { get; set; }
            public string tipo { get; set; }
            public string codSro { get; set; }
            public string nome { get; set; }
        }


    }
}
