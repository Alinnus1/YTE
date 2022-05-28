using YTE.Common.Exceptions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace YTE.Common.Extensions
{
    public static class Validation
    {
        public static void ThenThrow(this ValidationResult result, object model)
        {
            if (!result.IsValid)
            {
                //aici am setat sa nu ma mai deranjeze
                throw new ValidationErrorException(result, model);
            }
        }
    }
}
