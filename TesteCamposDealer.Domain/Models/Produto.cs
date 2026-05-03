using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteCamposDealer.Models
{
     [Table("Produto")]
 public class Produto
 {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.None)]
     public Guid idProduto { get; set; }

     [Required]
     [StringLength(100)]
     public string dscProduto { get; set; }

     public decimal vlrProduto { get; set; }

     public virtual ICollection<VendaItem> VendaItens { get; set; } = new List<VendaItem>();
     public virtual ICollection<ProdutoPrecoHistorico> Historico { get; set; } = new List<ProdutoPrecoHistorico>();
 }
}
