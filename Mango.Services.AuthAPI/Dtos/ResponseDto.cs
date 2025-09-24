namespace Mango.Services.AuthAPI.Dtos
{
    public class ResponseDto<T>
    {
        public T? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Transaction Complete";
    }
}
