using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteCamposDealer.Models
{
    [Table("Venda")]
public class Venda
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid idVenda { get; set; }

    public Guid idCliente { get; set; }

    public decimal vlrTotal { get; set; }

    public DateTime dthRegistro { get; set; }

    [ForeignKey("idCliente")]
    public virtual Cliente Cliente { get; set; }

    public virtual ICollection<VendaItem> Itens { get; set; } = new List<VendaItem>();
}
}
