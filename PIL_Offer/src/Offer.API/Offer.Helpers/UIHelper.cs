using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Offer.Helpers
{
    public class UIHelper
    {
        public static HttpResponseMessage CreateRequest(string baseurl, HttpMethod method, string relativeUrl,
            string jsonObj = null, string lang = "", string basicAuthUser = "", string basicAuthPassword = "")
        {
            using (var client = new HttpClient())
            {
                client.Timeout.Add(new TimeSpan(0, 0, 0, 15));
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(basicAuthUser) && !string.IsNullOrWhiteSpace(basicAuthPassword))
                {
                    var byteArray = new UTF8Encoding().GetBytes(basicAuthUser + ":" + basicAuthPassword);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                //client.DefaultRequestHeaders.Add("Accept-Language", lang);

                HttpRequestMessage request = new HttpRequestMessage(method, relativeUrl);

                if (jsonObj != null)
                    request.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                var Res = new HttpResponseMessage();
                try
                {
                    Res = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }

                return Res;
            }
        }

        public static string HtmlToPlainText(string htmlString)
        {
            var text = Regex.Replace(htmlString, @"<(.|\n)*?>", "");
            return text;
        }

        public static HttpResponseMessage AddRequestToServiceApi(string url, string json)
        {
            var Res = new HttpResponseMessage();
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    Res = client.PostAsync(url, content).Result;
                    if (Res.StatusCode != HttpStatusCode.OK)
                    {
                        try
                        {
                            var result = Res.Content.ReadAsStringAsync().Result;
                            var x = result.ToString();
                            LogHelper.LogException(x, "from radwan");
                            LogHelper.LogException(url, "from radwan");
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogException(ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Res;
        }

        public static string Upload(string url, IFormFile file,string objectId)
        {
            var Res = new HttpResponseMessage();
            var result = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    if (file.Length > 0)
                    {
                        client.DefaultRequestHeaders.Clear();
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        //form.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            //string s = Convert.ToBase64String(fileBytes);
                            //// act on the Base64 data
                            ByteArrayContent bytes = new ByteArrayContent(fileBytes);
                            form.Add(bytes, "file", file.FileName);
                            form.Add(new StringContent("Test"), "serviceId");
                            form.Add(new StringContent("Offers"), "serviceName");
                            if (!string.IsNullOrWhiteSpace(objectId))
                            {
                                objectId = GenerateGuid();
                            }
                            form.Add(new StringContent(objectId), "objectId");
                        }
                        Res = client.PostAsync(url, form).Result;
                        //if (Res.StatusCode != HttpStatusCode.OK)
                        //{
                            try
                            {
                                result = Res.Content.ReadAsStringAsync().Result;
                               LogHelper.LogException("S3 Result", result);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.LogException(ex.Message, ex.StackTrace);
                            }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return result;
        }

        private static string GenerateGuid()
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            return guidString;
        }
    }
}