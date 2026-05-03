using FluentValidation;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesteCamposDealer.Application.Exceptions;

namespace TesteCamposDealer.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                return;

            var ex = context.Exception;
            int statusCode;
            object body;

            switch (ex)
            {
                case NotFoundException notFound:
                    statusCode = (int)HttpStatusCode.NotFound;
                    body = new { message = notFound.Message };
                    break;

                case BusinessRuleException businessRule:
                    statusCode = (int)HttpStatusCode.Conflict;
                    body = new { message = businessRule.Message };
                    break;

                case ValidationException validation:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    body = new { errors = validation.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage)) };
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    body = new { message = "Ocorreu um erro interno no servidor." };
                    break;
            }

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult { Data = body, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            context.ExceptionHandled = true;
        }
    }
}