using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Common.Validation.Objects
{
    public class Result : IResult
    {
        protected Result() { }

        public bool Failed { get; protected set; }

        public string Message { get; protected set; }

        public bool Succeeded { get; protected set; }

        public static IResult Fail() => new Result { Failed = true, Succeeded = false };
        public static IResult Fail(string message) => new Result { Failed = true, Succeeded = false, Message = message };
        public static IResult Success() => new Result { Failed = false, Succeeded = true };
        public static IResult Success(string message) => new Result { Failed = false, Succeeded = true, Message = message };

    }
}
