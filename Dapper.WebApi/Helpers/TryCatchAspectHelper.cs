using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.WebApi.Helpers
{
    public static class TryCatchAspectHelper
    {
        public delegate Task<T> ParamsAction<T>(params string[] args);

        public static async Task<ObjectResult> ExecuteActionAndLogError<T>(ParamsAction<T> actionCallback)
        {
            ObjectResult statusCodeResult = default;

            try
            {
                T response = await actionCallback();
                statusCodeResult = new OkObjectResult(response);
            }
            // Filter by InnerException.
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                // Handle timeout.
                //Logger.Error("Request Timed out: " + ex.Message);
                statusCodeResult = CreateCodeResultFromException(ex.Message, 400);
                return statusCodeResult;
            }
            catch (TaskCanceledException ex)
            {
                // Handle cancellation.
                //Logger.Error("Request Canceled: " + ex.Message);
                statusCodeResult = CreateCodeResultFromException(ex.Message, 400);
                return statusCodeResult;
            }
            catch (AggregateException ae)
            {
                //_logger.LogError("SendAddAppEmail AggregateException");
                foreach (var ex in ae.InnerExceptions)
                {
                    //_logger.LogError($"SendAddAppEmail InnerException: {ex.Message} \n StackTrace: {ex.StackTrace}");
                }
            }
            catch (Exception ex)
            {
                //log error
                statusCodeResult = CreateCodeResultFromException(ex.Message, 500);
                return statusCodeResult;
            }

            return statusCodeResult;
        }

        private static ObjectResult CreateCodeResultFromException(string message, int statusCode)
        {
            var exceptionResult = new ObjectResult(message);
            exceptionResult.StatusCode = statusCode;
            return exceptionResult;
        }
    }
}
