using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteCamposDealer.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid idCliente { get; set; }

        [Required]
        [StringLength(200)]
        public string nomeCliente { get; set; }

        [Required]
        [StringLength(30)]
        public string endereco { get; set; }

        public DateTime dthRegistro { get; set; }

        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}
