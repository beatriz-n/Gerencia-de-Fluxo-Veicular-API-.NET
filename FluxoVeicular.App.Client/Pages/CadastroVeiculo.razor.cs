using FluxoVeicular.App.Client.Bases;
using FluxoVeicular.App.Client.Request;
using FluxoVeicular.App.Client.Response;
using FluxoVeicular.App.Client.ServiceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages
{
    public partial class CadastroVeiculo
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "placa")]
        public string? PlacaCadastro { get; set; }

        private readonly VeiculoServiceApi _veiculoApi;
        private readonly ISnackbar _snackbar;

        private MudForm? _form;
        private VeiculoResponse _veiculo = new();

        public CadastroVeiculo(VeiculoServiceApi veiculoApi, ISnackbar snackbar)
        {
            _veiculoApi = veiculoApi;
            _snackbar = snackbar;
        }

        // Chamado toda vez que a chave da URL 
        protected override async Task OnParametersSetAsync()
        {

            _veiculo = new VeiculoResponse { Placa = PlacaCadastro }; // novo cadastro
            StateHasChanged(); 
        }

        private async Task SalvarVeiculo()
        {
            if (_form is not null)
            {
                await _form.Validate();
                if (_form.IsValid)
                {
                    var criado = await _veiculoApi.CreateVeiculoAsync(new VeiculoRequest
                    {
                        Placa = _veiculo.Placa,
                        Cor = _veiculo.Cor
                    });

                    _snackbar.Add($"Veículo {_veiculo.Placa} cadastrado com sucesso!", Severity.Success);

                    _veiculo = criado;
                }
            }
        }
    }
}
