namespace FluxoVeicular.App.Client.Response.Dashboards
{
    public class DashboardsTotaisResponse
    {
        public int TotalEntradasHoje { get; set; }
        public int TotalSaidasHoje { get; set; }
        public int TotalVeiculosGaragem { get; set; }
        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
    }
}
