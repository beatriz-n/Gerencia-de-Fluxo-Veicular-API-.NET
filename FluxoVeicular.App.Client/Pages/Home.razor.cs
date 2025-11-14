using FluxoVeicular.App.Client.Response.Dashboards;
using FluxoVeicular.App.Client.ServiceApi;
using Microsoft.AspNetCore.Components;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class Home
    {
        public DashboardsTotaisResponse? DashboardsResponse { get; set; }
        [Inject] private DashboardsServiceApi DashboardsServiceApi { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DashboardsResponse = await DashboardsServiceApi.GetDashboardsTotaisHojeAsync() ?? new DashboardsTotaisResponse();
        }
    }
}
