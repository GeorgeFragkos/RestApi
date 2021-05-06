using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
