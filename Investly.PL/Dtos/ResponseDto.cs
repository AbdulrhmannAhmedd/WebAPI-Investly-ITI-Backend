namespace Investly.PL.Dtos
{
    public class ResponseDto<T> where T : class
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public bool RefreshTokenRequired { get; set; } = false;


    }
   
}
