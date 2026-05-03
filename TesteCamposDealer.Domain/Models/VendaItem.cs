using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace TesteCamposDealer.Models
{
    [Table("VendaItem")]
    public class VendaItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid idVendaItem { get; set; }

        public Guid idVenda { get; set; }

        public Guid idProduto { get; set; }

        public int quantidade { get; set; }

        public decimal vlrUnitario { get; set; }

        public decimal vlrTotal { get; set; }

        [ForeignKey("idVenda")]
        public virtual Venda Venda { get; set; }

        [ForeignKey("idProduto")]
        public virtual Produto Produto { get; set; }
    }
}