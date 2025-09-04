using FluxoVeicular.App.Client.Enum;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FluxoVeicular.App.Client.Pages;

public partial class DialogSolicitacao
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public string Mensagem { get; set; } = string.Empty;
    [Parameter] public TipoTela Tipo { get; set; }
}
