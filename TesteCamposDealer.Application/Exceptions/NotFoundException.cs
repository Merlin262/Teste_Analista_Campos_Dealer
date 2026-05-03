using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object id)
            : base($"{entityName} '{id}' não encontrado.") { }

        public NotFoundException(string message)
            : base(message) { }
    }
}
