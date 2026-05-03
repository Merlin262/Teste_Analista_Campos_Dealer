using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ClienteViewModel
    {
        public Guid idCliente { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(200)]
        [Display(Name = "Nome")]
        public string nomeCliente { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        [StringLength(30)]
        [Display(Name = "Endereço")]
        public string endereco { get; set; }

        [Display(Name = "Cadastrado em")]
        public DateTime dthRegistro { get; set; }
    }
}