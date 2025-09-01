using FluxoVeicular.App.Client.Response;
using FluxoVeicular.ServiceDefaults.Responses;
using FluxoVeicular.Web.ServiceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class Veiculo
    {
        private List<VeiculoResponse> _veiculos = new();
        private bool Loading = true;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        public VeiculoServiceApi VeiculoApi { get; set; } = default!;

        [Inject]
        public HubConnection Hub { get; set; } = default!;

        private void EditarVeiculo(Guid id)
        {
            Navigation.NavigateTo($"/veiculos/editar/{id}");
        }

        private void VisualizarVeiculo(Guid id)
        {
            Navigation.NavigateTo($"/veiculos/visualiza/{id}");
        }

        private async Task ExcluirVeiculo(Guid id)
        {
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
                //StateHasChanged();
            }
            else
            {
                Snackbar.Add("Erro ao excluir veículo.", Severity.Error);
            }
        }

        private async Task MostrarDialogoSolicitacao(object dados)
        {

            var parameters = new DialogParameters
            {
                ["Mensagem"] = $"Placa {dados} não cadastrada no sistema está solicitando acesso.\nDeseja cadastrar o veículo?"
            };

            var options = new DialogOptions
            {
                CloseButton = false,
                BackdropClick = false,
            };

            var dialog = DialogService.Show<DialogSolicitacao>("🚨 Alerta de Consulta", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (result.Data?.ToString() == "Cadastrar")
                {
                    Navigation.NavigateTo("/veiculos/cadastro/");
                }
                else
                {
                    Snackbar.Add("Acesso negado à solicitação.", Severity.Warning);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            // Conectar Hub
            if (Hub.State == HubConnectionState.Disconnected)
            {
                await Hub.StartAsync();
            }

            // Escutar evento de alerta
            Hub.On<object>("AlertaPlaca", async (dados) =>
            {
                if (dados is null) return;
                await MostrarDialogoSolicitacao(dados);
            });

            // Carregar veículos inicialmente
            _veiculos = await VeiculoApi.GetVeiculosAsync();
            Loading = false;
        }
    }
}
