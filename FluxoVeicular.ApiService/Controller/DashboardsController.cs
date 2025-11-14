using FluxoVeicular.App.Client.Response.Dashboards;
using FluxoVeicular.Infra.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluxoVeicular.ApiService.Controller
{
    [ApiController]
    [Route("api/veiculos/dashboards")]
    public class DashboardsController : ControllerBase
    {
        private readonly DashboardService _service;
        public DashboardsController(DashboardService service)
        {
            _service = service;
        }

        [HttpGet("totais")]
        public async Task<DashboardsTotaisResponse> DashboardsTotaisHojeAsync()
        {
            return await _service.GetDashboardsTotalHojeAsync();

        }
    }
}
