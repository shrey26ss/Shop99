using Entities.Enums;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ResponsePG<T> : IResponse<T>
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }
        public Dictionary<string, string> KeyVals { get; set; }

        public ResponsePG()
        {
            StatusCode = ResponseStatus.Failed;
            ResponseText = ResponseStatus.Failed.ToString();
        }
    }

    public class ResponsePG : IResponse
    {
        public ResponseStatus StatusCode { get; set; } = ResponseStatus.Failed;
        public string ResponseText { get; set; } = ResponseStatus.Failed.ToString();
        public ResponsePG()
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
   

}
