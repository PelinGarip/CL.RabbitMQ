namespace CL.RabbitMQ.Shared
{
    public class Result
    {
        public Result(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class Result<T>
        where T: class, new()
    {
        public Result(bool isSuccess, string message, T data)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
