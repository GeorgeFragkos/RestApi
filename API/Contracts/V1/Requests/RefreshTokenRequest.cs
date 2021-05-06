using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Contracts.V1.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
