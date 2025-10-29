using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Gromi.Infra.Utils.Helpers
{
    /// <summary>
    /// 校验帮助类
    /// </summary>
    public static class VerifyHelper
    {
        /// <summary>
        /// 身份证号格式校验
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool VerifyIDCard(string idCard)
        {
            // 身份证号正则表达式（适用于中国身份证）
            string pattern = @"^(?:\d{15}|\d{17}[\dXx])$";
            return Regex.IsMatch(idCard, pattern);
        }

        /// <summary>
        /// 手机号格式校验
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool VerifyPhone(string phone)
        {
            // 中国手机号正则表达式
            string pattern = @"^1[3-9]\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }

        /// <summary>
        /// 邮箱格式校验
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool VerifyEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}