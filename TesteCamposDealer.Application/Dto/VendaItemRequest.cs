using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Application.Dto
{
    public class VendaItemRequest
    {
        public Guid idProduto { get; set; }
        public int quantidade { get; set; }
    }
}
