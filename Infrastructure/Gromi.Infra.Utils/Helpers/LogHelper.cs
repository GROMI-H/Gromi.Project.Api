namespace Gromi.Infra.Utils.Helpers
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class LogHelper
    {
        public static string logPath = string.Empty;
        private static readonly object lockObj = new object(); // 用于多线程安全

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path"></param>
        public static void Initialize(string path)
        {
            logPath = path;
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="msg"></param>

        public static void Debug(string msg)
        {
            Write(GetLogFilePath(LogLevel.Debug), msg);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            Write(GetLogFilePath(LogLevel.Debug), msg);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            Write(GetLogFilePath(LogLevel.Error), msg);
        }

        /// <summary>
        /// 生成日志文件
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string GetLogFilePath(LogLevel level)
        {
            string logDirectory = Path.Combine(logPath, level.ToString());
            string dayDirectory = Path.Combine(logDirectory, DateTime.Now.ToString("yyyy-MM-dd"));
            string hourFile = Path.Combine(dayDirectory, DateTime.Now.ToString("yyyyMMdd-HH") + ".txt");

            CreateDirectory(logDirectory);
            CreateDirectory(dayDirectory);

            return hourFile;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directoryName"></param>
        private static void CreateDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        private static void Write(string fileName, string msg)
        {
            lock (lockObj) // 确保线程安全
            {
                try
                {
                    using (var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        byte[] dataArray = System.Text.Encoding.UTF8.GetBytes($"【{DateTime.Now:yyyy-MM-dd HH:mm:ss}】 {msg}\r");
                        fs.Seek(0, SeekOrigin.End);
                        fs.Write(dataArray, 0, dataArray.Length);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Logging Error: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试信息
        /// </summary>
        Debug,

        /// <summary>
        /// 日常记录信息
        /// </summary>
        Info,

        /// <summary>
        /// 错误信息
        /// </summary>
        Error
    }
}