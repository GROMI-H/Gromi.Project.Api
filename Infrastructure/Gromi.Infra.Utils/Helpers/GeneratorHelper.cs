using System.Text;

namespace Gromi.Infra.Utils.Helpers
{
    public static class GeneratorHelper
    {
        // 定义可用于生成字符串的字符集
        private static readonly char[] chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        /// <summary>
        /// 生成指定长度的随机字符串
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns>随机生成的字符串</returns>
        public static string GenerateRandomString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("长度必须大于0", nameof(length));
            }

            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                // 随机选择字符并添加到结果中
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}