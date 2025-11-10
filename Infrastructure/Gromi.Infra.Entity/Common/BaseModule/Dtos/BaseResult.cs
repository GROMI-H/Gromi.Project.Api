using Gromi.Infra.Entity.Common.BaseModule.Enums;

namespace Gromi.Infra.Entity.Common.BaseModule.Dtos
{
    /// <summary>
    /// 通用响应结构
    /// </summary>
    public class BaseResult
    {
        public ResponseCodeEnum Code { get; set; }

        public string Message { get; set; } = string.Empty;

        public BaseResult()
        {
            Code = ResponseCodeEnum.Success;
        }

        public BaseResult(ResponseCodeEnum code, string msg)
        {
            Code = code;
            Message = msg;
        }
    }

    /// <summary>
    /// 通用响应结构-带参
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResult<T> : BaseResult
    {
        public T Data { get; set; }

        public int DataCount => Data is System.Collections.ICollection collection ? collection.Count : (Data != null ? 1 : 0);

        public BaseResult() : base()
        {
        }

        public BaseResult(T data) : base()
        {
            Data = data;
        }

        public BaseResult(ResponseCodeEnum code, string msg, T data = default) : base(code, msg)
        {
            Code = code;
            Data = data;
            Message = msg;
        }
    }
}