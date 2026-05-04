using MediatR;
using Swashbuckle.Swagger.Annotations;
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
        [SwaggerResponse(HttpStatusCode.OK, "Lista paginada de clientes.", typeof(PagedResultViewModel<ClienteViewModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllClientesQuery(page));
            return Ok(result.ToPagedViewModel(c => c.ToViewModel()));
        }

        /// <summary>Retorna os dados de um cliente pelo seu identificador único.</summary>
        /// <param name="id">Identificador único do cliente (GUID).</param>
        [SwaggerResponse(HttpStatusCode.OK, "Cliente encontrado.", typeof(ClienteViewModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Cliente não encontrado.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var c = await _mediator.Send(new GetClienteByIdQuery(id));
            return Ok(c.ToViewModel());
        }

        /// <summary>Cadastra um novo cliente. Retorna o objeto persistido com status 201 Created.</summary>
        /// <param name="vm">Dados do cliente: nome e endereço.</param>
        [SwaggerResponse(HttpStatusCode.Created, "Cliente cadastrado com sucesso.", typeof(ClienteViewModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos.", typeof(ApiValidationErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(ClienteViewModel vm)
        {
            var result = await _mediator.Send(new CreateClienteCommand { nomeCliente = vm.nomeCliente, endereco = vm.endereco });
            return Content(HttpStatusCode.Created, result.ToViewModel());
        }

        /// <summary>Atualiza os dados de um cliente existente. Retorna o objeto atualizado.</summary>
        /// <param name="id">Identificador único do cliente (GUID).</param>
        /// <param name="vm">Novos dados do cliente: nome e endereço.</param>
        [SwaggerResponse(HttpStatusCode.OK, "Cliente atualizado com sucesso.", typeof(ClienteViewModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos.", typeof(ApiValidationErrorResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Cliente não encontrado.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, ClienteViewModel vm)
        {
            var result = await _mediator.Send(new UpdateClienteCommand { idCliente = id, nomeCliente = vm.nomeCliente, endereco = vm.endereco });
            return Ok(result.ToViewModel());
        }

        /// <summary>Remove um cliente pelo seu identificador único. Retorna 204 No Content.</summary>
        /// <param name="id">Identificador único do cliente (GUID).</param>
        [SwaggerResponse(HttpStatusCode.NoContent, "Cliente removido com sucesso.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Cliente não encontrado.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteClienteCommand(id));
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
