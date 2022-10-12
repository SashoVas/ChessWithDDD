namespace Shared
{
    public class Result
    {
        public bool Succeeded { get;}
        public string Error { get; private set; }
        protected Result(bool success,string error)
        {
            if (success && error!=string.Empty)
            {
                throw new InvalidOperationException();
            }
            if (!success && error==string.Empty)
            {
                throw new InvalidOperationException();
            }
            Succeeded = success;
            Error = error;
        }
        public static Result Fail(string message)
        {
            return new(false,message);
        }
        public static Result Success()
        {
            return new(true, string.Empty);
        }
        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(false,message,default(T));
        }
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(false, string.Empty, value);
        }
    }
    public class Result<T> : Result
    {
        public T Value { get; set; }
        public Result(bool success, string error, T value) : base(success, error)
        {
            Value = value;
        }
    }
}
