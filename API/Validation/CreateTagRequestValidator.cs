using API.Contracts.V1.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Validation
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.TagName)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}
