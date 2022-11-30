using Entities.Enums;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Response<T> : IResponse<T>
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }

        public Response()
        {
            StatusCode = ResponseStatus.Failed;
            ResponseText = ResponseStatus.Failed.ToString();
        }
    }

    public class Response : IResponse
    {
        public ResponseStatus StatusCode { get; set; } = ResponseStatus.Failed;
        public string ResponseText { get; set; } = ResponseStatus.Failed.ToString();
        public Response()
        {
            this.StatusCode = ResponseStatus.Failed;
            this.ResponseText = ResponseStatus.Failed.ToString();
        }
    }
    public class Request<T> : IRequest<T>
    {
        public string AuthToken { get; set; }
        public T Param { get; set; }
    }

   //#region Response Interface

   // public interface IResponse<T>
   // {
   //     ResponseStatus StatusCode { get; set; }
   //     string ResponseText { get; set; }
   //     Exception Exception { get; set; }
   //     T Result { get; set; }
   // }

   // public interface IResponse
   // {
   //     ResponseStatus StatusCode { get; set; }
   //     string ResponseText { get; set; }
   // }

   // public interface IRequest<T>
   // {
   //     string AuthToken { get; set; }
   //     T Param { get; set; }
   // }

   // #endregion
}
