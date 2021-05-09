using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
