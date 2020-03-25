using System;
using System.Runtime.Serialization;

namespace ThePage.Api
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }

        public ApiError ApiError { get; set; }
    }
}