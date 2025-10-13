using FluxoVeicular.App.Client.Bases;
using FluxoVeicular.App.Client.Response;
using FluxoVeicular.App.Client.ServiceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class Veiculo : BaseComponente
    {
        private List<VeiculoResponse> _veiculos = new();
        private bool Loading = true;

        // Serviço específico da página — injetado por propriedade
        [Inject] private VeiculoServiceApi VeiculoApi { get; set; } = default!;

        private void EditarVeiculo(Guid id) =>
            Navigation.NavigateTo($"/veiculos/editar/{id}");

        private void VisualizarVeiculo(Guid id) =>
            Navigation.NavigateTo($"/veiculos/visualiza/{id}");

        private async Task ExcluirVeiculo(Guid id)
        {
            bool confirmado = await DialogService.ShowMessageBox(
                "Confirmação",
                "Tem certeza que deseja excluir este veículo?",
                yesText: "Sim", cancelText: "Não"
            ) == true;

            if (!confirmado) return;

            var sucesso = await VeiculoApi.DeleteVeiculoAsync(id);
            if (sucesso)
            {
                Snackbar.Add("Veículo excluído com sucesso.", Severity.Success);
                _veiculos = await VeiculoApi.GetVeiculosAsync();
            }
            else
            {
                Snackbar.Add("Erro ao excluir veículo.", Severity.Error);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync(); // inicializa Hub/listeners do BaseComponente
            _veiculos = await VeiculoApi.GetVeiculosAsync();
            Loading = false;
        }
    }
}
