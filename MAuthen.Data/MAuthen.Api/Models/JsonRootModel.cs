using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Models;

namespace MAuthen.Api.Models
{
    public class JsonRootModel
    {
        public ServiceSettings AuthenticationRequest { get; set; }
    }
}
