using MediatR;
using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
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
            if (p == null) { Response.StatusCode = 404; return Json(new { message = $"Produto '{id}' não encontrado." }, JsonRequestBehavior.AllowGet); }
            return Json(p.ToViewModel(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Create(ProdutoViewModel vm)
        {
            try
            {
                var result = await _mediator.Send(new CreateProdutoCommand { dscProduto = vm.dscProduto, vlrProduto = vm.vlrProduto });
                Response.StatusCode = 201;
                return Json(result.ToViewModel());
            }
            catch (ValidationException ex)
            {
                Response.StatusCode = 400;
                return Json(new { errors = ex.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage)) });
            }
        }

        [HttpPut, Route("{id}")]
        public async Task<ActionResult> Update(Guid id, ProdutoViewModel vm)
        {
            try
            {
                var result = await _mediator.Send(new UpdateProdutoCommand { idProduto = id, dscProduto = vm.dscProduto, vlrProduto = vm.vlrProduto });
                if (result == null) { Response.StatusCode = 404; return Json(new { message = $"Produto '{id}' não encontrado." }); }
                return Json(result.ToViewModel());
            }
            catch (ValidationException ex)
            {
                Response.StatusCode = 400;
                return Json(new { errors = ex.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage)) });
            }
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteProdutoCommand(id));
                if (!result) { Response.StatusCode = 404; return Json(new { message = $"Produto '{id}' não encontrado." }, JsonRequestBehavior.AllowGet); }
                Response.StatusCode = 204;
                return new EmptyResult();
            }
            catch (InvalidOperationException ex)
            {
                Response.StatusCode = 409;
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
