using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace TesteCamposDealer.Models
{
    [Table("ProdutoPrecoHistorico")]
    public class ProdutoPrecoHistorico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid idHistorico { get; set; }

        public Guid idProduto { get; set; }

        public decimal vlrAnterior { get; set; }

        public decimal vlrNovo { get; set; }

        public DateTime dthAlteracao { get; set; }

        [ForeignKey("idProduto")]
        public virtual Produto Produto { get; set; }
    }
}