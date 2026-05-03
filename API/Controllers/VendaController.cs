using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
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
    public class VendaController : Controller
    {
        private readonly IMediator _mediator;
        public VendaController(IMediator mediator) { _mediator = mediator; }

        [HttpGet, Route("")]
        public async Task<ActionResult> GetAll(int page = 1)
        {
            var result = await _mediator.Send(new GetAllVendasQuery(page));
            return Json(result.ToPagedViewModel(v => v.ToViewModel()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("ranking")]
        public async Task<ActionResult> GetRanking()
        {
            var result = await _mediator.Send(new GetRankingQuery());
            return Json(result.Select(v => v.ToViewModel()).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("cliente/{idCliente}")]
        public async Task<ActionResult> GetByCliente(Guid idCliente)
        {
            var result = await _mediator.Send(new GetVendasByClienteQuery(idCliente));
            return Json(result.Select(v => v.ToViewModel()).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var v = await _mediator.Send(new GetVendaByIdQuery(id));
            return Json(v.ToViewModel(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Create(VendaFormViewModel vm)
        {
            var result = await _mediator.Send(new CreateVendaCommand
            {
                idCliente = vm.idCliente,
                itens = vm.Itens?.Select(i => new VendaItemRequest { idProduto = i.idProduto, quantidade = i.quantidade }).ToList()
                          ?? new List<VendaItemRequest>()
            });
            Response.StatusCode = 201;
            return Json(result.ToViewModel());
        }

        [HttpPut, Route("{id}")]
        public async Task<ActionResult> Update(Guid id, VendaFormViewModel vm)
        {
            var result = await _mediator.Send(new UpdateVendaCommand
            {
                idVenda = id,
                itens = vm.Itens?.Select(i => new VendaItemRequest { idProduto = i.idProduto, quantidade = i.quantidade }).ToList()
                        ?? new List<VendaItemRequest>()
            });
            return Json(result.ToViewModel());
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteVendaCommand(id));
            Response.StatusCode = 204;
            return new EmptyResult();
        }
    }
}
