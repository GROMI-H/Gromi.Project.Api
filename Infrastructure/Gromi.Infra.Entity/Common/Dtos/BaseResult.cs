using Gromi.Infra.Entity.Common.Enums;

namespace Gromi.Infra.Entity.Common.Dtos
{
    /// <summary>
    /// 通用响应结构
    /// </summary>
    public class BaseResult
    {
        public ResponseCodeEnum Code { get; set; }

        public string Msg { get; set; } = string.Empty;

        public BaseResult()
        {
            Code = ResponseCodeEnum.Success;
        }

        public BaseResult(ResponseCodeEnum code, string msg)
        {
            Code = code;
            Msg = msg;
        }
    }

    /// <summary>
    /// 通用响应结构-带参
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResult<T> : BaseResult
    {
        public T? Data { get; set; }

        public int DataCount { get; set; }

        public BaseResult() : base()
        {
        }

        public BaseResult(T data) : base()
        {
            Data = data;
            if (data is System.Collections.ICollection collection)
            {
                DataCount = collection.Count;
            }
            else
            {
                DataCount = data != null ? 1 : 0;
            }
        }

        public BaseResult(ResponseCodeEnum code, string msg, T? data = default) : base(code, msg)
        {
            Data = data;
            if (data is System.Collections.ICollection collection)
            {
                DataCount = collection.Count;
            }
            else
            {
                DataCount = data != null ? 1 : 0;
            }
        }
    }
}