using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static ApiResponse<T> SuccessResponse(T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                ErrorMessage = null
            };
        }

        public static ApiResponse<T> ErrorResponse(string errorMessage)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default(T),
                ErrorMessage = errorMessage
            };
        }
    }

}
