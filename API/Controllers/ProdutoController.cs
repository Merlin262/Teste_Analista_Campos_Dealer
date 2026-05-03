using MediatR;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TesteCamposDealer.Application.Handlers.Produtos.Commands;
using TesteCamposDealer.Application.Handlers.Produtos.Commands.DeleteProduto;
using TesteCamposDealer.Application.Handlers.Produtos.Commands.UpdateProduto;
using TesteCamposDealer.Application.Handlers.Produtos.Queries.GetAllProdutos;
using TesteCamposDealer.Application.Handlers.Produtos.Queries.GetProdutoById;
using TesteCamposDealer.Mappers;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Controllers
{
    [RoutePrefix("api/produtos")]
    public class ProdutoController : ApiController
    {
        private readonly IMediator _mediator;
        public ProdutoController(IMediator mediator) { _mediator = mediator; }

        /// <summary>Retorna a lista paginada de produtos cadastrados.</summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllProdutosQuery(page));
            return Ok(result.ToPagedViewModel(p => p.ToViewModel()));
        }

        /// <summary>Retorna os dados de um produto pelo seu identificador único.</summary>
        /// <param name="id">Identificador único do produto (GUID).</param>
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var p = await _mediator.Send(new GetProdutoByIdQuery(id));
            return Ok(p.ToViewModel());
        }

        /// <summary>Cadastra um novo produto com descrição e valor unitário. Retorna o objeto persistido com status 201 Created. O valor é registrado no histórico de preços para rastreabilidade.</summary>
        /// <param name="vm">Dados do produto: descrição e valor unitário.</param>
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(ProdutoViewModel vm)
        {
            var result = await _mediator.Send(new CreateProdutoCommand { dscProduto = vm.dscProduto, vlrProduto = vm.vlrProduto });
            return Content(HttpStatusCode.Created, result.ToViewModel());
        }

        /// <summary>Atualiza a descrição e o valor de um produto existente. O novo valor é registrado no histórico de preços para garantir rastreabilidade financeira. Retorna o objeto atualizado.</summary>
        /// <param name="id">Identificador único do produto (GUID).</param>
        /// <param name="vm">Novos dados do produto: descrição e valor unitário.</param>
        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, ProdutoViewModel vm)
        {
            var result = await _mediator.Send(new UpdateProdutoCommand { idProduto = id, dscProduto = vm.dscProduto, vlrProduto = vm.vlrProduto });
            return Ok(result.ToViewModel());
        }

        /// <summary>Remove um produto pelo seu identificador único. Retorna 204 No Content.</summary>
        /// <param name="id">Identificador único do produto (GUID).</param>
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProdutoCommand(id));
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
