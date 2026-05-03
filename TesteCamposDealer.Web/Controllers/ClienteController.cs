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
    public class ClienteController : Controller
    {
        private readonly ApiClient _api;
        public ClienteController(ApiClient api) { _api = api; }

        public async Task<ActionResult> Index(int page = 1)
        {
            var result = await _api.GetAllClientesAsync(page);
            return View(result);
        }

        public ActionResult Create() => View(new ClienteViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClienteViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await _api.CreateClienteAsync(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var vm = await _api.GetClienteByIdAsync(id);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, ClienteViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await _api.UpdateClienteAsync(id, vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _api.DeleteClienteAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}