using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewModels
{
    public class BuscaViewModel
    {
        public BuscaViewModel(IList<Produto> itens, bool houveResultados)
        {
            this.Itens = itens;
            this.HouveResultados = houveResultados;
        }

        public BuscaViewModel(bool houveResultados)
        {
            HouveResultados = houveResultados;
        }

        public IList<Produto> Itens { get; set; }
        public string Pesquisa { get; set; }
        public bool HouveResultados { get; set; }

    }
}
