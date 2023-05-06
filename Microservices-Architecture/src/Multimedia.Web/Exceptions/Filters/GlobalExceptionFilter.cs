using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Multimedia.Web.Exceptions.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public GlobalExceptionFilter(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            var response = new ViewResult();

            response.ViewName = "InternalServerError";
            response.StatusCode = 500;
            response.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
            response.ViewData["Exception"] = context.Exception;

            if (context.Exception is BadRequestException)
            {
                response.ViewName = "BadRequest";
                response.ViewData["Exception"] = context.Exception;
                response.StatusCode = 400;
            }
            if (context.Exception is NotFoundException)
            {
                response.ViewName = "NotFound";
                response.ViewData["Exception"] = context.Exception;
                response.StatusCode = 404;
            }
            if (context.Exception is ConflictException)
            {
                response.ViewName = "Conflict";
                response.ViewData["Exception"] = context.Exception;
                response.StatusCode = 409;
            }
            if (context.Exception is UnauthorizedException)
            {
                response.ViewName = "Unauthorized";
                response.ViewData["Exception"] = context.Exception;
                response.StatusCode = 401;
            }

            context.Result = response;
            base.OnException(context);
        }
    }
}
