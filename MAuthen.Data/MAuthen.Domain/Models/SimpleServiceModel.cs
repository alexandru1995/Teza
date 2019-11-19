using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Models
{
    public class SimpleServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
