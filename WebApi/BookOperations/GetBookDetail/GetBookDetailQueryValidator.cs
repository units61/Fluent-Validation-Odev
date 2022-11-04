using FluentValidation;

namespace WebApi.BookOperations.GetBookDetail
{
   public class GetBookDetailQeuryValidator : AbstractValidator<GetBookDetailQuery>
   {
        public GetBookDetailQeuryValidator()
        {
            RuleFor(command => command.BookId).GreaterThan(0);
            
        }
   }
}