using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TesteCamposDealer.Application.Handlers.Clientes.Commands;
using TesteCamposDealer.Application.Handlers.Clientes.Commands.DeleteCliente;
using TesteCamposDealer.Application.Handlers.Clientes.Commands.UpdateCliente;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetClienteById;
using TesteCamposDealer.Mappers;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Controllers
{
    [RoutePrefix("api/clientes")]
    public class ClienteController : Controller
    {
        private readonly IMediator _mediator;
        public ClienteController(IMediator mediator) { _mediator = mediator; }

        [HttpGet, Route("")]
        public async Task<ActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllClientesQuery(page));
            return Json(result.ToPagedViewModel(c => c.ToViewModel()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var c = await _mediator.Send(new GetClienteByIdQuery(id));
            return Json(c.ToViewModel(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Create(ClienteViewModel vm)
        {
            var result = await _mediator.Send(new CreateClienteCommand { nomeCliente = vm.nomeCliente, endereco = vm.endereco });
            Response.StatusCode = 201;
            return Json(result.ToViewModel());
        }

        [HttpPut, Route("{id}")]
        public async Task<ActionResult> Update(Guid id, ClienteViewModel vm)
        {
            var result = await _mediator.Send(new UpdateClienteCommand { idCliente = id, nomeCliente = vm.nomeCliente, endereco = vm.endereco });
            return Json(result.ToViewModel());
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteClienteCommand(id));
            Response.StatusCode = 204;
            return new EmptyResult();
        }
    }
}
