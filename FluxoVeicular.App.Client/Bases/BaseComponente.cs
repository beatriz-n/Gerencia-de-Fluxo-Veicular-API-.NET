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
            string valorMensagem = jsonObject.GetProperty("mensagem").GetString() ?? string.Empty;
            string tipoAcessoStr = jsonObject.GetProperty("tipoAcesso").GetString() ?? "Entrada";

            DialogParameters parameters;
            DialogOptions options = new DialogOptions
            {
                CloseButton = false,
                BackdropClick = false,
            };

            IDialogReference dialog;

            if (valorDados == 1)
            {
                if (tipoAcessoStr == "Saida")
                    return;
                // Acesso Liberado
                parameters = new DialogParameters
                {
                    ["Mensagem"] = $"Placa {valorMensagem} é reconhecida e já cadastrada na base de dados!",
                    ["Tipo"] = TipoTela.AcessoLiberado
                };

                dialog = DialogService.Show<DialogSolicitacao>("✅ Acesso Liberado", parameters, options);
            }
            else
            {
                // Acesso Negado / Solicitação de cadastro
                parameters = new DialogParameters
                {
                    ["Mensagem"] = $"Placa {valorMensagem} não cadastrada no sistema está solicitando acesso.\nDeseja cadastrar o veículo?",
                    ["Tipo"] = TipoTela.SolicitacaoAcesso
                };

                dialog = DialogService.Show<DialogSolicitacao>("🚨 Alerta de Consulta", parameters, options);
            }

            // Fecha automaticamente após 30s
            _ = FecharDialogoAutomaticamenteAsync(dialog, 30);

            var result = await dialog.Result;

            // Tratar resultado do modal
            if (result.Data?.ToString() == "FechadoAutomatico")
            {
                Snackbar.Add("A solicitação foi fechada automaticamente após 30s.", Severity.Info);
            }
            else if (valorDados == 1 && result.Data?.ToString() == "Liberado")
            {
                Snackbar.Add("Acesso Liberado!", Severity.Success);
            }
            else if (valorDados != 1 && !result.Canceled)
            {
                if (result.Data?.ToString() == "Cadastrar")
                {
                    Navigation.NavigateTo($"/veiculos/cadastro?placa={valorMensagem}");
                }
                else
                {
                    Snackbar.Add("Acesso negado à solicitação.", Severity.Warning);
                }
            }
        }

        private async Task FecharDialogoAutomaticamenteAsync(IDialogReference dialog, int segundos)
        {
            await Task.Delay(segundos * 1000);
            await InvokeAsync(() => dialog.Close(DialogResult.Ok("FechadoAutomatico")));
        }

        protected override async Task OnInitializedAsync()
        {
            // Cria o HubConnection
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
