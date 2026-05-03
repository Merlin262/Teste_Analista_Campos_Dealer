using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
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
    public class ProdutoController : Controller
    {
        private readonly IMediator _mediator;
        public ProdutoController(IMediator mediator) { _mediator = mediator; }

        [HttpGet, Route("")]
        public async Task<ActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllProdutosQuery(page));
            return Json(result.ToPagedViewModel(p => p.ToViewModel()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var p = await _mediator.Send(new GetProdutoByIdQuery(id));
            return Json(p.ToViewModel(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Create(ProdutoViewModel vm)
        {
            var result = await _mediator.Send(new CreateProdutoCommand { dscProduto = vm.dscProduto, vlrProduto = vm.vlrProduto });
            Response.StatusCode = 201;
            return Json(result.ToViewModel());
        }

        [HttpPut, Route("{id}")]
        public async Task<ActionResult> Update(Guid id, ProdutoViewModel vm)
        {
            var result = await _mediator.Send(new UpdateProdutoCommand { idProduto = id, dscProduto = vm.dscProduto, vlrProduto = vm.vlrProduto });
            return Json(result.ToViewModel());
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProdutoCommand(id));
            Response.StatusCode = 204;
            return new EmptyResult();
        }
    }
}
