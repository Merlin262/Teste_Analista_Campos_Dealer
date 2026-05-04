using FluentValidation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using TesteCamposDealer.Application.Exceptions;

namespace TesteCamposDealer.Filters
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var ex = context.Exception;
            HttpStatusCode statusCode;
            object body;

            switch (ex)
            {
                case NotFoundException notFound:
                    statusCode = HttpStatusCode.NotFound;
                    body = new { message = notFound.Message };
                    break;

                case BusinessRuleException businessRule:
                    statusCode = HttpStatusCode.Conflict;
                    body = new { message = businessRule.Message };
                    break;

                case ValidationException validation:
                    statusCode = HttpStatusCode.BadRequest;
                    body = new
                    {
                        errors = validation.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage))
                    };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    body = new { message = "Ocorreu um erro interno no servidor." };
                    break;
            }

            context.Result = new ErrorActionResult(context.Request, statusCode, body);
        }
    }

    internal class ErrorActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly HttpStatusCode _statusCode;
        private readonly object _body;

        public ErrorActionResult(HttpRequestMessage request, HttpStatusCode statusCode, object body)
        {
            _request = request;
            _statusCode = statusCode;
            _body = body;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(_statusCode, _body);
            return Task.FromResult(response);
        }
    }
}
