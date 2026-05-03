using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteCamposDealer.Web.ViewModels
{
    public class VendaFormViewModel
    {
        public Guid? idVenda { get; set; }

        [Required(ErrorMessage = "Selecione um cliente.")]
        [Display(Name = "Cliente")]
        public Guid idCliente { get; set; }

        public List<VendaItemInputViewModel> Itens { get; set; } = new List<VendaItemInputViewModel>();

        public string Erro { get; set; }

        public IEnumerable<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();
        public IEnumerable<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
    }

    public class VendaItemInputViewModel
    {
        [Required]
        public Guid idProduto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade mínima é 1.")]
        public int quantidade { get; set; }
    }
}