using FluxoVeicular.ServiceDefaults.Responses;
using FluxoVeicular.Web.ServiceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class VisualizaVeiculo
    {
        [Inject]
        private VeiculoServiceApi _veiculoApi { get; set; } = default!;
        [Inject]
        private ISnackbar _snackbar { get; set; } = default!;

        private MudForm? _form;
        private VeiculoResponse? _veiculo = new();

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Parameter] public Guid Id { get; set; }


        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                _snackbar.Add("Veículo inválido.", Severity.Error);
                Navigation.NavigateTo("/veiculos");
                return;
            }

            var veiculoApiResult = await _veiculoApi.GetVeiculoByIdAsync(Id);
            if (veiculoApiResult != null)
                _veiculo = veiculoApiResult;
            else
            {
                _snackbar.Add("Veículo não encontrado.", Severity.Error);
                Navigation.NavigateTo("/veiculos");
            }
        }
    }
}
