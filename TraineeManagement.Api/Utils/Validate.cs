using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using TraineeManagement.Api.DTOs;

namespace TraineeManagement.Api.Utils
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                IEnumerable<Errors> errorDetails = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new Errors
                    {
                        Field = x.Key,
                        Message = x.Value.Errors.First().ErrorMessage
                    });
                ApiErrorResponseDTO response = new ApiErrorResponseDTO
                {
                    Errors = errorDetails,
                    Success = true
                };
                context.Result = new BadRequestObjectResult(response);

            }
        }
    }
}