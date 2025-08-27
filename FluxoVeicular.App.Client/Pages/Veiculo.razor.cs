using FluxoVeicular.ServiceDefaults.Responses;
using FluxoVeicular.Web.ServiceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class Veiculo
    {
        private List<VeiculoResponse> _veiculos = new();

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        [Inject]
        public VeiculoServiceApi VeiculoApi { get; set; } = default!;

        private void EditarVeiculo(Guid id)
        {
            Navigation.NavigateTo($"/veiculos/editar/{id}");
        }
        private async Task ExcluirVeiculo(Guid id)
        {
            Console.WriteLine($"[DEBUG] Clicou para excluir {id}");
            bool confirmado = await DialogService.ShowMessageBox(
                "Confirmação",
                "Tem certeza que deseja excluir este veículo?",
                yesText: "Sim", cancelText: "Não") == true;

            if (!confirmado) return;

            var sucesso = await VeiculoApi.DeleteVeiculoAsync(id);
            if (sucesso)
            {
                Snackbar.Add("Veículo excluído com sucesso.", Severity.Success);
                _veiculos = await VeiculoApi.GetVeiculosAsync();
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Erro ao excluir veículo.", Severity.Error);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _veiculos = await VeiculoApi.GetVeiculosAsync();
        }
    }
}
