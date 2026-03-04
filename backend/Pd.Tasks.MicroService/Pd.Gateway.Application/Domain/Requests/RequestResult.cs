using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Pd.Tasks.Application.Domain.Requests
{
    public class RequestResult<TData>
    {
        public bool IsSuccessful { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public TData Data { get; set; }
    }


    public class RequestResult : RequestResult<object>
    {
        public static RequestResult<TData> Success<TData>(TData data)
        {
            return new RequestResult<TData>
            {
                IsSuccessful = true,
                StatusCode = 200,
                Data = data
            };
        }

        public static RequestResult<TData> BadRequest<TData>(string errorMessage)
        {
            return new RequestResult<TData>
            {
                IsSuccessful = false,
                StatusCode = 400,
                ErrorMessage = errorMessage
            };
        }

        public static RequestResult<TData> NotFound<TData>(string errorMessage)
        {
            return new RequestResult<TData>
            {
                IsSuccessful = false,
                StatusCode = 404,
                ErrorMessage = errorMessage
            };
        }

        public static RequestResult<TData> InternalServerError<TData>(string errorMessage)
        {
            return new RequestResult<TData>
            {
                IsSuccessful = false,
                StatusCode = 500,
                ErrorMessage = errorMessage
            };
        }
    }
}