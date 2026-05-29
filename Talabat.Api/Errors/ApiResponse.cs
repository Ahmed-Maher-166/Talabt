namespace Talabat.APIS.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch 
            {
               200 => "Success",
                201 => "Created",
                400 => "Bad Request",
                404 => "Not Found",
                500 => "Server Error",
                _ => null
            };
        }
    }
}
