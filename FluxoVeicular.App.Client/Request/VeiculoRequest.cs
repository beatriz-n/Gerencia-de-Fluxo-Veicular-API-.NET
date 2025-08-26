using System.Text.Json.Serialization;

namespace FluxoVeicular.App.Client.Request
{
    public class VeiculoRequest
    {

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Placa { get; set; }
        public string? Cor { get; set; }
    }
}
