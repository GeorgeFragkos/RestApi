using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Authorization
{
    public class WorksForCompanyRequirment : IAuthorizationRequirement
    {
        public string DomainName { get;  }

        public WorksForCompanyRequirment(string domainName)
        {
            DomainName = domainName;
        }
    }
}
