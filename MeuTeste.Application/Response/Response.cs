using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MeuTeste.Application.Response
{
    public class Response
    {
        protected Response()
        {
            Messages = new HashSet<string>();
        }

        protected Response(string message) : this()
        {
            Messages.Add(message);
        }

        protected Response(IEnumerable<string> messages) : this()
        {
            Messages.UnionWith(messages);
        }

        //public ErrorResponse ErrorResponse { get; private set; }
        [JsonIgnore] public bool IsFailure => !IsSuccess;
        [JsonIgnore] public bool IsSuccess => Messages.Count == 0;
        public ISet<string> Messages { get; }

        public static Response Fail(string message)
        {
            return new Response(message);
        }

        public static Response Fail(IEnumerable<string> messages)
        {
            return new Response(messages);
        }

        // public static Response Fail(Error error)
        // {
        //     var response = new Response();
        //     response.Messages.Add(error.Message);
        //     //response.ErrorResponse = new ErrorResponse(error);

        //     return response;
        // }

        public static Response Ok()
        {
            return new Response();
        }
    }

    public class Response<TValue> : Response
    {
        public Response()
        {
        }

        public Response(TValue value)
        {
            Value = value;
        }

        private Response(string message)
            : base(message) { }

        private Response(IEnumerable<string> messages)
            : base(messages) { }

        //public new ErrorResponse ErrorResponse { get; private set; }
        public TValue Value { get; }

        public new static Response<TValue> Fail(string message)
        {
            return new Response<TValue>(message);
        }

        public new static Response<TValue> Fail(IEnumerable<string> messages)
        {
            return new Response<TValue>(messages);
        }

        // public new static Response<TValue> Fail(Error error)
        // {
        //     var response = new Response<TValue>();
        //     response.Messages.Add(error.Message);
        //     response.ErrorResponse = new ErrorResponse(error);

        //     return response;
        // }

        public static Response<TValue> Ok(TValue value)
        {
            return new Response<TValue>(value);
        }
    }
}