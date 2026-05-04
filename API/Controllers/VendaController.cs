using MediatR;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TesteCamposDealer.Application.Dto;
using TesteCamposDealer.Application.Handlers.Vendas.Commands.CreateVenda;
using TesteCamposDealer.Application.Handlers.Vendas.Commands.DeleteVenda;
using TesteCamposDealer.Application.Handlers.Vendas.Commands.UpdateVenda;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetAllVendas;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetRanking;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendaById;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente;
using TesteCamposDealer.Mappers;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Controllers
{
    [RoutePrefix("api/vendas")]
    public class VendaController : ApiController
    {
        private readonly IMediator _mediator;
        public VendaController(IMediator mediator) { _mediator = mediator; }

        /// <summary>Retorna a lista paginada de vendas. Cada venda contém o cliente, a lista de itens com quantidade e valor unitário, e o valor total sumarizado.</summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        [SwaggerResponse(HttpStatusCode.OK, "Lista paginada de vendas.", typeof(PagedResultViewModel<VendaViewModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllVendasQuery(page));
            return Ok(result.ToPagedViewModel(v => v.ToViewModel()));
        }

        /// <summary>Retorna o ranking paginado das maiores vendas realizadas, ordenado pelo valor total decrescente.</summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        [SwaggerResponse(HttpStatusCode.OK, "Ranking paginado de vendas.", typeof(PagedResultViewModel<VendaViewModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpGet, Route("ranking")]
        public async Task<IHttpActionResult> GetRanking(int page = 1)
        {
            var result = await _mediator.Send(new GetRankingQuery(page));
            return Ok(result.ToPagedViewModel(v => v.ToViewModel()));
        }

        /// <summary>Retorna as vendas paginadas vinculadas a um cliente específico, incluindo itens e valor total de cada venda.</summary>
        /// <param name="idCliente">Identificador único do cliente (GUID).</param>
        /// <param name="page">Número da página (padrão: 1).</param>
        [SwaggerResponse(HttpStatusCode.OK, "Vendas do cliente.", typeof(PagedResultViewModel<VendaViewModel>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Cliente não encontrado.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpGet, Route("cliente/{idCliente}")]
        public async Task<IHttpActionResult> GetByCliente(Guid idCliente, int page = 1)
        {
            var result = await _mediator.Send(new GetVendasByClienteQuery(idCliente, page));
            return Ok(result.ToPagedViewModel(v => v.ToViewModel()));
        }

        /// <summary>Retorna os dados de uma venda pelo seu identificador único, incluindo todos os itens e o valor total.</summary>
        /// <param name="id">Identificador único da venda (GUID).</param>
        [SwaggerResponse(HttpStatusCode.OK, "Venda encontrada.", typeof(VendaViewModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Venda não encontrada.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var v = await _mediator.Send(new GetVendaByIdQuery(id));
            return Ok(v.ToViewModel());
        }

        /// <summary>Registra uma nova venda vinculada a um cliente. Cada item deve informar o produto e a quantidade. O valor unitário é capturado do cadastro de produtos no momento da venda e preservado no histórico. Retorna o objeto persistido com status 201 Created.</summary>
        /// <param name="vm">Dados da venda: ID do cliente e lista de itens (idProduto, quantidade).</param>
        [SwaggerResponse(HttpStatusCode.Created, "Venda registrada com sucesso.", typeof(VendaViewModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos.", typeof(ApiValidationErrorResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Cliente ou produto não encontrado.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.Conflict, "Regra de negócio violada.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(VendaFormViewModel vm)
        {
            var result = await _mediator.Send(new CreateVendaCommand
            {
                idCliente = vm.idCliente,
                itens = vm.Itens?.Select(i => new VendaItemRequest { idProduto = i.idProduto, quantidade = i.quantidade }).ToList()
                          ?? new List<VendaItemRequest>()
            });
            return Content(HttpStatusCode.Created, result.ToViewModel());
        }

        /// <summary>Atualiza os itens de uma venda existente. O valor unitário de cada produto é recapturado no momento da atualização. Retorna o objeto atualizado.</summary>
        /// <param name="id">Identificador único da venda (GUID).</param>
        /// <param name="vm">Nova lista de itens da venda (idProduto, quantidade).</param>
        [SwaggerResponse(HttpStatusCode.OK, "Venda atualizada com sucesso.", typeof(VendaViewModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos.", typeof(ApiValidationErrorResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Venda ou produto não encontrado.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.Conflict, "Regra de negócio violada.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, VendaFormViewModel vm)
        {
            var result = await _mediator.Send(new UpdateVendaCommand
            {
                idVenda = id,
                itens = vm.Itens?.Select(i => new VendaItemRequest { idProduto = i.idProduto, quantidade = i.quantidade }).ToList()
                        ?? new List<VendaItemRequest>()
            });
            return Ok(result.ToViewModel());
        }

        /// <summary>Remove uma venda pelo seu identificador único. Retorna 204 No Content.</summary>
        /// <param name="id">Identificador único da venda (GUID).</param>
        [SwaggerResponse(HttpStatusCode.NoContent, "Venda removida com sucesso.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Venda não encontrada.", typeof(ApiErrorResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno no servidor.", typeof(ApiErrorResponse))]
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteVendaCommand(id));
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
