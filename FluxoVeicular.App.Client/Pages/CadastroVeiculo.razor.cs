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
        // Recebe a placa via query string
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

        protected override void OnInitialized()
        {
            // Se veio placa pela URL, já popula o objeto
            if (!string.IsNullOrEmpty(PlacaCadastro))
            {
                _veiculo.Placa = PlacaCadastro;
            }
        }

        private async Task SalvarVeiculo()
        {
            if (_form is not null)
            {
                await _form.Validate();
                if (_form.IsValid)
                {
                    _snackbar.Add($"Veículo {_veiculo.Placa} cadastrado com sucesso!", Severity.Success);

                    _veiculo = await _veiculoApi.CreateVeiculoAsync(new VeiculoRequest
                    {
                        Placa = _veiculo.Placa,
                        Cor = _veiculo.Cor
                    });
                }
            }
        }
    }
}
