using System.Net;
using System.Net.Mail;

namespace Gromi.Infra.Utils.Helpers
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public class EmailHelper
    {
        private static readonly string SmtpServer = "smtp.163.com";
        private static readonly int SmtpPort = 465;
        private static readonly string UserName = "gromi_x@163.com";
        private static readonly string Password = "UPSLXfAMwprz5Bg7";

        public static void SendEmail()
        {
            using (var client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true; // 启用 SSL
                client.Credentials = new NetworkCredential(UserName, Password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(UserName),
                    Subject = "测试邮件",
                    Body = "测试邮件文本",
                    IsBodyHtml = false, // 如果邮件体为HTML则设置为true
                };
                mailMessage.To.Add("hwj20010504@163.com");

                try
                {
                    client.Send(mailMessage);
                    Console.WriteLine("邮件发送成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"邮件发送失败: {ex.Message}");
                }
            }
        }
    }
}