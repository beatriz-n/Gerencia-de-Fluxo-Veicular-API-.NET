using System.Net.Http.Json;
using FluxoVeicular.App.Client.Response.Dashboards;

namespace FluxoVeicular.App.Client.ServiceApi
{
    public class DashboardsServiceApi
    {
        private readonly HttpClient _http;

        public DashboardsServiceApi(HttpClient http)
        {
            _http = http;
        }

        private const string BaseUrl = "https://localhost:4040/api/veiculos/dashboards";

        public async Task<DashboardsTotaisResponse?> GetDashboardsTotaisHojeAsync()
        {
            return await _http.GetFromJsonAsync<DashboardsTotaisResponse>($"{BaseUrl}/totais");
        }

    }
}
