using FluxoVeicular.App.Client.Request;
using FluxoVeicular.ServiceDefaults.Responses;
using System.Net.Http.Json;

namespace FluxoVeicular.Web.ServiceApi
{
    public class VeiculoServiceApi
    {
        private readonly HttpClient _http;

        public VeiculoServiceApi(HttpClient http)
        {
            _http = http;
        }

        private const string BaseUrl = "https://localhost:4040/api/veiculos";


        // GET: Lista todos os veículos
        public async Task<List<VeiculoResponse>> GetVeiculosAsync()
        {
            var result = await _http.GetFromJsonAsync<List<VeiculoResponse>>(BaseUrl);
            return result ?? new List<VeiculoResponse>();
        }

        // GET: Busca veículo por ID
        public async Task<VeiculoResponse?> GetVeiculoByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<VeiculoResponse>($"{BaseUrl}/{id}");
        }

        // GET: Busca veículo por Placa
        public async Task<VeiculoResponse?> GetVeiculoByPlacaAsync(string placa)
        {
            return await _http.GetFromJsonAsync<VeiculoResponse>($"{BaseUrl}/placa/{placa}");
        }

        // POST: Cria veículo
        public async Task<VeiculoResponse?> CreateVeiculoAsync(VeiculoRequest request)
        {
            var response = await _http.PostAsJsonAsync(BaseUrl, request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<VeiculoResponse>();
        }

        // PUT: Atualiza veículo
        public async Task<bool> UpdateVeiculoAsync(Guid id, VeiculoRequest request)
        {
            var response = await _http.PutAsJsonAsync($"{BaseUrl}/{id}", request);
            return response.IsSuccessStatusCode;
        }

        // DELETE: Remove veículo
        public async Task<bool> DeleteVeiculoAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

