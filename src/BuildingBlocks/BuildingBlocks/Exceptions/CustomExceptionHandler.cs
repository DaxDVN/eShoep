using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions
{
  public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      logger.LogError(
          "Error Message: {exceptionMessage}, Time of occurence {time}", exception.Message, DateTime.UtcNow);

      (string Details, string Title, int Statuscode) details = exception switch
      {
        InternalServerException => (
            exception.Message
            , exception.GetType().Name
            , httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
        ),
        ValidationException => (
            exception.Message
            , exception.GetType().Name
            , httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
        ),
        BadRequestException => (
            exception.Message
            , exception.GetType().Name
            , httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
        ),
        NotFoundException => (
            exception.Message
            , exception.GetType().Name
            , httpContext.Response.StatusCode = StatusCodes.Status404NotFound
        ),
        _ => (
            exception.Message
            , exception.GetType().Name
            , httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
        )
      };

      var problemDetails = new ProblemDetails
      {
        Title = details.Title,
        Detail = details.Details,
        Status = details.Statuscode,
        Instance = httpContext.Request.Path
      };

      problemDetails.Extensions.Add("tracedId", httpContext.TraceIdentifier);

      if (exception is ValidationException validationException)
      {
        problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
      }

      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
      return true;
    }
  }
}
