using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteCamposDealer.Web.ViewModels
{
    public class VendaViewModel
    {
        public Guid idVenda { get; set; }
        public Guid idCliente { get; set; }
        public string nomeCliente { get; set; }
        public string enderecoCliente { get; set; }
        public decimal vlrTotal { get; set; }
        public DateTime dthRegistro { get; set; }
        public List<VendaItemViewModel> itens { get; set; } = new List<VendaItemViewModel>();
    }

    public class VendaItemViewModel
    {
        public Guid idVendaItem { get; set; }
        public Guid idProduto { get; set; }
        public string dscProduto { get; set; }
        public int quantidade { get; set; }
        public decimal vlrUnitario { get; set; }
        public decimal vlrTotal { get; set; }
    }
}