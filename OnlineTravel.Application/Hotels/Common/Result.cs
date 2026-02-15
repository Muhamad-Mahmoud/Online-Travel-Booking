using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string ErrorMessage { get; }
        public List<string> ValidationErrors { get; }

        private Result(bool isSuccess, T data, string errorMessage, List<string> validationErrors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors ?? new List<string>();
        }

        public static Result<T> Success(T data) => new(true, data, null);
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
        public static Result<T> ValidationFailure(List<string> errors) => new(false, default, "Validation failed", errors);
    }

}
