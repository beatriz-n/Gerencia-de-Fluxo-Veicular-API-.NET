using FluxoVeicular.App.Client.Enum;
using FluxoVeicular.App.Client.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Text.Json;

namespace FluxoVeicular.App.Client.Bases
{
    public abstract class BaseComponente : ComponentBase, IAsyncDisposable
    {
        // Removido [Inject] HubConnection
        private HubConnection Hub { get; set; } = default!;

        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        private IDisposable? _alertSubscription;

        protected virtual async Task MostrarDialogoSolicitacao(object dados)
        {
            string jsonString = dados.ToString();

            var jsonObject = JsonSerializer.Deserialize<JsonElement>(jsonString);

            int valorDados = jsonObject.GetProperty("dados").GetInt32();
            string valorMensagem = jsonObject.GetProperty("mensagem").GetString();

            if (valorDados == 1)
            {
                var parameters = new DialogParameters
                {
                    ["Mensagem"] = $"Placa {valorMensagem} é reconhecida e já cadastrada na base de dados!",
                    ["Tipo"] = TipoTela.AcessoLiberado
                };

                var options = new DialogOptions
                {
                    CloseButton = false,
                    BackdropClick = false,
                };

                var dialog = DialogService.Show<DialogSolicitacao>("✅ Acesso Liberado", parameters, options);
                var result = await dialog.Result;
                if (result.Data?.ToString() == "Liberado")
                {
                    Snackbar.Add("Acesso negado à solicitação.", Severity.Success);
                }

            }
            else
            {
                var parameters = new DialogParameters
                {
                    ["Mensagem"] = $"Placa {valorMensagem} não cadastrada no sistema está solicitando acesso.\nDeseja cadastrar o veículo?",
                    ["Tipo"] = TipoTela.SolicitacaoAcesso
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
        }

        protected override async Task OnInitializedAsync()
        {
            // Cria o HubConnection localmente
            Hub = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("https://localhost:4040/hub/notificacao"))
                .WithAutomaticReconnect()
                .Build();

            Hub.On<object>("AlertaPlaca", async dados =>
            {
                if (dados is null) return;
                await MostrarDialogoSolicitacao(dados);
            });

            await Hub.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                _alertSubscription?.Dispose();
                if (Hub != null && Hub.State == HubConnectionState.Connected)
                {
                    await Hub.StopAsync();
                }
            }
            catch
            {
                // swallow ou log — evitando exceções em dispose
            }
        }
    }
}
