using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ProdutoViewModel
    {
        public Guid idProduto { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        [StringLength(100)]
        [Display(Name = "Descrição")]
        public string dscProduto { get; set; }

        [Required(ErrorMessage = "Preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        [Display(Name = "Preço (R$)")]
        public decimal vlrProduto { get; set; }
    }
}