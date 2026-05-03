using MediatR;
using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
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
            if (c == null) { Response.StatusCode = 404; return Json(new { message = $"Cliente '{id}' não encontrado." }, JsonRequestBehavior.AllowGet); }
            return Json(c.ToViewModel(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Create(ClienteViewModel vm)
        {
            try
            {
                var result = await _mediator.Send(new CreateClienteCommand { nomeCliente = vm.nomeCliente, endereco = vm.endereco });
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
        public async Task<ActionResult> Update(Guid id, ClienteViewModel vm)
        {
            try
            {
                var result = await _mediator.Send(new UpdateClienteCommand { idCliente = id, nomeCliente = vm.nomeCliente, endereco = vm.endereco });
                if (result == null) { Response.StatusCode = 404; return Json(new { message = $"Cliente '{id}' não encontrado." }); }
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
                var result = await _mediator.Send(new DeleteClienteCommand(id));
                if (!result) { Response.StatusCode = 404; return Json(new { message = $"Cliente '{id}' não encontrado." }, JsonRequestBehavior.AllowGet); }
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