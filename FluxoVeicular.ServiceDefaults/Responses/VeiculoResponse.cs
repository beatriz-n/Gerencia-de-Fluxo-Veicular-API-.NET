using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoVeicular.ServiceDefaults.Responses
{
    public class VeiculoResponse
    {
        public Guid Id { get; set; }
        public bool Acesso { get; set; }
        public string? Placa { get; set; }
        public string? Cor { get; set; }
    }
}
