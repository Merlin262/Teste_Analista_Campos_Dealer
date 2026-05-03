using MediatR;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
    public class ClienteController : ApiController
    {
        private readonly IMediator _mediator;
        public ClienteController(IMediator mediator) { _mediator = mediator; }

        /// <summary>Retorna a lista paginada de clientes cadastrados.</summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllClientesQuery(page));
            return Ok(result.ToPagedViewModel(c => c.ToViewModel()));
        }

        /// <summary>Retorna os dados de um cliente pelo seu identificador único.</summary>
        /// <param name="id">Identificador único do cliente (GUID).</param>
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var c = await _mediator.Send(new GetClienteByIdQuery(id));
            return Ok(c.ToViewModel());
        }

        /// <summary>Cadastra um novo cliente. Retorna o objeto persistido com status 201 Created.</summary>
        /// <param name="vm">Dados do cliente: nome e endereço.</param>
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(ClienteViewModel vm)
        {
            var result = await _mediator.Send(new CreateClienteCommand { nomeCliente = vm.nomeCliente, endereco = vm.endereco });
            return Content(HttpStatusCode.Created, result.ToViewModel());
        }

        /// <summary>Atualiza os dados de um cliente existente. Retorna o objeto atualizado.</summary>
        /// <param name="id">Identificador único do cliente (GUID).</param>
        /// <param name="vm">Novos dados do cliente: nome e endereço.</param>
        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, ClienteViewModel vm)
        {
            var result = await _mediator.Send(new UpdateClienteCommand { idCliente = id, nomeCliente = vm.nomeCliente, endereco = vm.endereco });
            return Ok(result.ToViewModel());
        }

        /// <summary>Remove um cliente pelo seu identificador único. Retorna 204 No Content.</summary>
        /// <param name="id">Identificador único do cliente (GUID).</param>
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteClienteCommand(id));
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
