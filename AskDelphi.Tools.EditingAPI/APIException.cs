using System;
using System.Runtime.Serialization;

namespace AskDelphi.Tools.EditingAPI
{
    [Serializable]
    public class APIException : Exception
    {
        public string ErrorCode;
        public string ErrorMessage;

        public APIException()
        {
        }

        public APIException(string message) : base(message)
        {
        }

        public APIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public APIException(string v, string errorCode, string errorMessage) : base($"{v}, api-code={errorCode}, api-message={errorMessage}")
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        protected APIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}