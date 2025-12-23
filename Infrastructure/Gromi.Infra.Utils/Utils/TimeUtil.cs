namespace Gromi.Infra.Utils.Utils
{
    /// <summary>
    /// 时间处理工具
    /// </summary>
    public static class TimeUtil
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static long GetTimestamp(DateTime time, TimestampEnum type = TimestampEnum.Second)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(time.ToUniversalTime());

            switch (type)
            {
                case TimestampEnum.Second:
                    return dateTimeOffset.ToUnixTimeSeconds();

                case TimestampEnum.Millisecond:
                    return dateTimeOffset.ToUnixTimeMilliseconds();

                case TimestampEnum.Microsecond:
                    return dateTimeOffset.ToUnixTimeMilliseconds() * 1000;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "非法时间戳类型");
            }
        }
    }

    /// <summary>
    /// 时间戳类型枚举
    /// </summary>
    public enum TimestampEnum
    {
        /// <summary>
        /// 秒
        /// </summary>
        Second,

        /// <summary>
        /// 毫秒
        /// </summary>
        Millisecond,

        /// <summary>
        /// 微秒
        /// </summary>
        Microsecond
    }
}