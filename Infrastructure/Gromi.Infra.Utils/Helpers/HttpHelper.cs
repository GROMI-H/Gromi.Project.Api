using System.Text;

namespace Gromi.Infra.Utils.Helpers
{
    /// <summary>
    /// Http请求帮助类
    /// </summary>
    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// 发送GET请求
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应内容</returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string>? headers = null)
        {
            try
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(header.Key);
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("GET请求失败", ex);
            }
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="jsonContent">请求体</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应内容</returns>
        public static async Task<string> PostAsync(string url, string jsonContent, Dictionary<string, string>? headers = null)
        {
            try
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(header.Key);
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("POST请求失败", ex);
            }
        }
    }
}