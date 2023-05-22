using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Authentication
{
    public class ExternalAuthenticationRequest
    {
        public string? Provider { get; set; }
        public string? IdToken { get; set; }
    }
}
