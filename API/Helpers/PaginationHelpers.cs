using Api.Contracts.V1.Requests.Queries;
using Api.Contracts.V1.Responses;
using API.Contracts.V1.Responses;
using API.Domain;
using API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginationResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response)
        {
            var nextPage = pagination.PageNumber >= 1 ? 
                uriService.GetAllPostUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
                : null;
            var prevPage = pagination.PageNumber - 1 >= 1 ? 
                uriService.GetAllPostUri(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString()
               : null;

         return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = prevPage

            };
        }
    }
}
