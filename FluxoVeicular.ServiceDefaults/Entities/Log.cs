namespace FluxoVeicular.ServiceDefaults.Entities
{
    public class Log
    {
        public Guid Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public DateTime DataHora { get; set; } = DateTime.Now;
        public string Tipo { get; set; } = string.Empty;
    }
}
