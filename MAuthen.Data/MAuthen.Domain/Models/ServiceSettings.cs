using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Models
{
    public class ServiceSettings
    {
        public Guid client_id { get; set; }
        public string issuer { get; set; }
        public string audiance { get; set; } = "localhost:5001";
        public string secret { get; set; }
    }
}
