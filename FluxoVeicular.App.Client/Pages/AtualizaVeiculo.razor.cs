using FluxoVeicular.App.Client.Response;
using FluxoVeicular.App.Client.ServiceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class AtualizaVeiculo
    {
        private bool Loading = true;
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
            Loading = false;
        }

        private async Task SalvarAlteracoes()
        {
            if (Id != Guid.Empty)
            {
                {
                    var atualizado = await _veiculoApi.UpdateVeiculoAsync(Id, new FluxoVeicular.App.Client.Request.VeiculoRequest
                    {
                        Placa = _veiculo!.Placa,
                        Cor = _veiculo.Cor
                    });
                    
                    if (atualizado != null)
                    {
                        _snackbar.Add("Veículo atualizado com sucesso!", Severity.Success);
                        //Navigation.NavigateTo("/veiculos");
                    }
                    else
                    {
                        _snackbar.Add("Erro ao atualizar veículo.", Severity.Error);
                    }
                }
            }

        }
    }
}
