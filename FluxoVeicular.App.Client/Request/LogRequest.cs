using System.Text.Json.Serialization;

namespace FluxoVeicular.App.Client.Request
{
    public class LogRequest
    {

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Placa { get; set; } = string.Empty;
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public string Tipo { get; set; } = string.Empty;
    }
}
