using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(string baseUrl)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // ---------- Clientes ----------

        public Task<PagedResultViewModel<ClienteViewModel>> GetAllClientesAsync(int page = 1)
            => GetAsync<PagedResultViewModel<ClienteViewModel>>($"api/clientes?page={page}");

        public Task<ClienteViewModel> GetClienteByIdAsync(Guid id)
            => GetAsync<ClienteViewModel>($"api/clientes/{id}");

        public Task<ClienteViewModel> CreateClienteAsync(ClienteViewModel vm)
            => PostAsync<ClienteViewModel>($"api/clientes", vm);

        public Task<ClienteViewModel> UpdateClienteAsync(Guid id, ClienteViewModel vm)
            => PutAsync<ClienteViewModel>($"api/clientes/{id}", vm);

        public Task DeleteClienteAsync(Guid id)
            => DeleteAsync($"api/clientes/{id}");

        // ---------- Produtos ----------

        public Task<PagedResultViewModel<ProdutoViewModel>> GetAllProdutosAsync(int page = 1)
            => GetAsync<PagedResultViewModel<ProdutoViewModel>>($"api/produtos?page={page}");

        public Task<ProdutoViewModel> GetProdutoByIdAsync(Guid id)
            => GetAsync<ProdutoViewModel>($"api/produtos/{id}");

        public Task<ProdutoViewModel> CreateProdutoAsync(ProdutoViewModel vm)
            => PostAsync<ProdutoViewModel>($"api/produtos", vm);

        public Task<ProdutoViewModel> UpdateProdutoAsync(Guid id, ProdutoViewModel vm)
            => PutAsync<ProdutoViewModel>($"api/produtos/{id}", vm);

        public Task DeleteProdutoAsync(Guid id)
            => DeleteAsync($"api/produtos/{id}");

        // ---------- Vendas ----------

        public Task<PagedResultViewModel<VendaViewModel>> GetAllVendasAsync(int page = 1)
            => GetAsync<PagedResultViewModel<VendaViewModel>>($"api/vendas?page={page}");

        public Task<VendaViewModel> GetVendaByIdAsync(Guid id)
            => GetAsync<VendaViewModel>($"api/vendas/{id}");

        public Task<PagedResultViewModel<VendaViewModel>> GetVendasByClienteAsync(Guid idCliente, int page = 1)
            => GetAsync<PagedResultViewModel<VendaViewModel>>($"api/vendas/cliente/{idCliente}?page={page}");

        public Task<PagedResultViewModel<VendaViewModel>> GetRankingAsync(int page = 1)
            => GetAsync<PagedResultViewModel<VendaViewModel>>($"api/vendas/ranking?page={page}");

        public Task<VendaViewModel> CreateVendaAsync(VendaFormViewModel vm)
            => PostAsync<VendaViewModel>("api/vendas", vm);

        public Task<VendaViewModel> UpdateVendaAsync(Guid id, VendaFormViewModel vm)
            => PutAsync<VendaViewModel>($"api/vendas/{id}", vm);

        public Task DeleteVendaAsync(Guid id)
            => DeleteAsync($"api/vendas/{id}");

        // ---------- helpers ----------

        private async Task<T> GetAsync<T>(string url)
        {
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task<T> PostAsync<T>(string url, object body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task<T> PutAsync<T>(string url, object body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task DeleteAsync(string url)
        {
            var response = await _http.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                string msg;
                try
                {
                    dynamic obj = JsonConvert.DeserializeObject(body);
                    msg = obj?.message != null ? (string)obj.message : $"Erro {(int)response.StatusCode}.";
                }
                catch
                {
                    msg = $"Erro {(int)response.StatusCode}.";
                }
                throw new InvalidOperationException(msg);
            }
        }
    }
}