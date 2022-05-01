using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.API
{
    public enum ResponseStatus
    {
        success,
        fail,
        error
    }

    public class ResponseBody<T>
    {
        public string Status { get; }
        public int Code { get; }
        public string Message { get; }
        public T Data { get; }

        protected internal ResponseBody(string status, int code, string message, T data)
        {
            Status = status;
            Code = code;
            Message = message;
            Data = data;
        }
    }

    public class ResponseBody : ResponseBody<string>
    {
        protected ResponseBody() : base(ResponseStatus.success.ToString(), StatusCodes.Status200OK, null, null)
        {
        }
        protected ResponseBody(string message) : base(ResponseStatus.error.ToString(), StatusCodes.Status500InternalServerError, message, null)
        {
        }

        public static ResponseBody<T> Ok<T>(T data)
        {
            return new ResponseBody<T>(ResponseStatus.success.ToString(), StatusCodes.Status200OK, null, data);
        }

        public static ResponseBody Ok()
        {
            return new ResponseBody();
        }

        public static ResponseBody Error(string message)
        {
            return new ResponseBody(message);
        }

        public static ResponseBody<T> Error<T>(T data)
        {
            return new ResponseBody<T>(ResponseStatus.fail.ToString(), StatusCodes.Status400BadRequest, null, data);
        }
    }
}
