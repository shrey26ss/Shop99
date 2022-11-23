using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppUtility.APIRequest
{
    public class AppWebRequest : IAppWebRequest
    {
        public static AppWebRequest O { get { return Instance.Value; } }
        private static Lazy<AppWebRequest> Instance = new Lazy<AppWebRequest>(() => new AppWebRequest());
        private AppWebRequest() { }

        public async Task<HttpResponse> PostAsync(string URL, string PostData, string ContentType = "application/json")
        {
            HttpResponse httpResponse = new HttpResponse();
            HttpWebRequest http = (HttpWebRequest)System.Net.WebRequest.Create(URL);
            http.Timeout = 5 * 60 * 1000;
            var data = Encoding.ASCII.GetBytes(PostData);
            http.Method = "POST";
            http.ContentType = ContentType;
            http.ContentLength = data.Length;
            using (Stream stream = await http.GetRequestStreamAsync().ConfigureAwait(false))
            {
                await stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            }
            string result = "";
            try
            {
                WebResponse response = await http.GetResponseAsync().ConfigureAwait(false);
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    httpResponse.HttpStatusCode = HttpStatusCode.OK;
                    httpResponse.Result = await sr.ReadToEndAsync().ConfigureAwait(false);
                    //result = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            catch (UriFormatException ufx)
            {
                throw new Exception(ufx.Message);
            }
            catch (WebException wx)
            {
                if (wx.Response != null)
                {
                    using (var ErrorResponse = wx.Response)
                    {
                        using (StreamReader sr = new StreamReader(ErrorResponse.GetResponseStream()))
                        {
                            
                            httpResponse.Result = await sr.ReadToEndAsync().ConfigureAwait(false);
                            httpResponse.HttpMessage = wx.Message;
                            httpResponse.HttpStatusCode = ((HttpWebResponse)wx.Response).StatusCode;
                            //result = await sr.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
                else
                {
                    throw new Exception(wx.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return httpResponse;
        }
    }
    public class MimePart
    {
        public NameValueCollection Headers { get; } = new NameValueCollection();

        public byte[] Header { get; private set; }

        public long GenerateHeaderFooterData(string boundary)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("--");
            stringBuilder.Append(boundary);
            stringBuilder.AppendLine();
            foreach (string key in Headers.AllKeys)
            {
                stringBuilder.Append(key);
                stringBuilder.Append(": ");
                stringBuilder.AppendLine(Headers[key]);
            }
            stringBuilder.AppendLine();

            Header = Encoding.UTF8.GetBytes(stringBuilder.ToString());

            return Header.Length + Data.Length + 2;
        }

        public Stream Data { get; set; }

    }
}
