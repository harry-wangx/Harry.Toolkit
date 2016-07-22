using System;

namespace Harry
{
    public class Result
    {
        public Result() { }

        public Result(int code)
        {
            this.Code = code;
        }
        public Result(int code, string msg)
            : this(code)
        {
            this.Msg = msg;
        }
        /// <summary>
        /// 返回结果,0为正常
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Msg { get; set; }

    }

    public class Result<T> : Result
    {
        public Result() { }

        public Result(int code)
            : base(code)
        {
        }
        public Result(int code, string msg)
            : base(code, msg)
        {
        }
        public Result(int code, T data)
            : base(code)
        {
            this.Data = data;
        }
        public Result(int code, T data, string msg)
            : base(code, msg)
        {
            this.Data = data;
        }

        public T Data { get; set; }

    }
}
