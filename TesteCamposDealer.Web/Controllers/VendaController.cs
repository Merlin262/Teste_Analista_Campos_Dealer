using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TesteCamposDealer.Web.Services;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Web.Controllers
{
    public class VendaController : Controller
    {
        private readonly ApiClient _api;
        public VendaController(ApiClient api) { _api = api; }

        public async Task<ActionResult> Index(int page = 1)
        {
            var vendasTask = _api.GetAllVendasAsync(page);
            var rankingTask = _api.GetRankingAsync();
            await Task.WhenAll(vendasTask, rankingTask);
            ViewBag.Ranking = rankingTask.Result.Data;
            return View(vendasTask.Result);
        }

        public async Task<ActionResult> PorCliente(Guid idCliente, int page = 1)
        {
            var clienteTask = _api.GetClienteByIdAsync(idCliente);
            var vendasTask = _api.GetVendasByClienteAsync(idCliente, page);
            await Task.WhenAll(clienteTask, vendasTask);
            ViewBag.Cliente = clienteTask.Result;
            return View(vendasTask.Result);
        }

        public async Task<ActionResult> Ranking(int page = 1)
        {
            var result = await _api.GetRankingAsync(page);
            return View(result);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var vm = await _api.GetVendaByIdAsync(id);
            return View(vm);
        }

        public async Task<ActionResult> Create()
        {
            var vm = await BuildFormViewModel(new VendaFormViewModel());
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendaFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(await BuildFormViewModel(vm));
            try
            {
                await _api.CreateVendaAsync(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                vm.Erro = ex.Message;
                return View(await BuildFormViewModel(vm));
            }
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var venda = await _api.GetVendaByIdAsync(id);
            var vm = new VendaFormViewModel
            {
                idVenda = venda.idVenda,
                idCliente = venda.idCliente
            };
            foreach (var item in venda.itens)
                vm.Itens.Add(new VendaItemInputViewModel { idProduto = item.idProduto, quantidade = item.quantidade });
            return View(await BuildFormViewModel(vm));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, VendaFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(await BuildFormViewModel(vm));
            try
            {
                await _api.UpdateVendaAsync(id, vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                vm.Erro = ex.Message;
                return View(await BuildFormViewModel(vm));
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _api.DeleteVendaAsync(id);
            return RedirectToAction("Index");
        }

        private async Task<VendaFormViewModel> BuildFormViewModel(VendaFormViewModel vm)
        {
            var clientesTask = _api.GetAllClientesAsync(1);
            var produtosTask = _api.GetAllProdutosAsync(1);
            await Task.WhenAll(clientesTask, produtosTask);
            vm.Clientes = clientesTask.Result.Data;
            vm.Produtos = produtosTask.Result.Data;
            return vm;
        }
    }
}