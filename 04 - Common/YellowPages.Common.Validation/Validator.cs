using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Validation.Objects;

namespace YellowPages.Common.Validation
{
    public abstract class Validator<T> : AbstractValidator<T>
    {
        private string Message { get; set; }

        public new IResult Validate(T instance)
        {
            if (instance == null)
            {
                return Result.Fail(Message ?? string.Empty);
            }

            var result = base.Validate(instance);

            if (result.IsValid)
            {
                return Result.Success();
            }

            return Result.Fail(Message ?? result.ToString());
        }

        public Task<IResult> ValidateAsync(T instance)
        {
            return Task.FromResult(Validate(instance));
        }

        public void WithMessage(string message)
        {
            Message = message;
        }
    }
}
