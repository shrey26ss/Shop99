using AppUtility.Helper;
using Entities.Enums;
using Infrastructure.Interface;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebApp.Models;

namespace WebApp.AppCode.UploadImageService
{
    public class UploadImageService: IUploadImageService
    {
        private readonly AppSettings _appsetting;
        private readonly IRequestInfo _rinfo;
        public UploadImageService(AppSettings appsetting, IRequestInfo requestInfo)
        {
            _appsetting = appsetting;
            _rinfo = requestInfo;
        }
        public Response Upload(FileUploadModel request)
        {
            var response = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var fileValidRes = Validate.O.IsFileValid(request.file);
            if (fileValidRes.StatusCode != ResponseStatus.Success)
            {
                response.StatusCode = fileValidRes.StatusCode;
                response.ResponseText = fileValidRes.ResponseText;
                return response;
            }
            try
            {
                if (_appsetting.CloudFlare != null && !string.IsNullOrEmpty(_appsetting.CloudFlare.BaseURL) && !string.IsNullOrEmpty(_appsetting.CloudFlare.ApiKey) && !string.IsNullOrEmpty(_appsetting.CloudFlare.Authorization))
                {
                    CloudFlareResponse res = new CloudFlareResponse();
                    using (var client = new HttpClient())
                    {
                        string URL = _appsetting.CloudFlare.BaseURL + _appsetting.CloudFlare.ApiKey + "/images/v1";
                        var cloudrequest = new HttpRequestMessage(HttpMethod.Post, URL);
                        cloudrequest.Headers.Add("Authorization", "Bearer " + _appsetting.CloudFlare.Authorization);
                        var content = new MultipartFormDataContent();
                        using (var ms = new MemoryStream())
                        {
                            request.file.CopyTo(ms);
                            var msArr = ms.ToArray();
                            var fileStream = new StreamContent(new MemoryStream(msArr));
                            content.Add(fileStream, "file", DateTime.Now.ToString("ddMMyyyyhhmmssff"));
                            cloudrequest.Content = content;
                            var cloudresponse = client.SendAsync(cloudrequest).Result;
                            if (cloudresponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                            {
                                response.StatusCode = ResponseStatus.Failed;
                                response.ResponseText = "Invalid Cloud Url!";
                            }
                            else
                            {
                                res = JsonConvert.DeserializeObject<CloudFlareResponse>(cloudresponse.Content.ReadAsStringAsync().Result);
                                if (res.success)
                                {
                                    response.StatusCode = ResponseStatus.Success;
                                    response.ResponseText = res.result.variants.FirstOrDefault();
                                }
                                else
                                {
                                    response.StatusCode = ResponseStatus.Failed;
                                    response.ResponseText = res.errors.FirstOrDefault().message;
                                }
                            }
                        }
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(request.FilePath);
                    if (!Directory.Exists(sb.ToString()))
                    {
                        Directory.CreateDirectory(sb.ToString());
                    }
                    var filename = ContentDispositionHeaderValue.Parse(request.file.ContentDisposition).FileName.Trim('"');
                    string originalExt = Path.GetExtension(filename).ToLower();
                    string[] Extensions = { ".png", ".jpeg", ".jpg", ".webp" };
                    if (!Extensions.Contains(originalExt))
                    {
                        response.StatusCode = ResponseStatus.Failed;
                        response.ResponseText = "You can only upload JPEG, JPG, and PNG files.";
                        return response;
                    }
                    if (string.IsNullOrEmpty(request.FileName))
                    {
                        request.FileName = filename;
                    }
                    sb.Append($"{request.FileName}");
                    using (FileStream fs = File.Create(sb.ToString()))
                    {
                        request.file.CopyTo(fs);
                        fs.Flush();
                    }
                    response.StatusCode = ResponseStatus.Success;
                    response.ResponseText = $"{_rinfo.GetBaseURI()}/{request.FilePath.Replace(Directory.GetCurrentDirectory(),"").Replace(@"\","").Replace("wwwroot/", "")}{request.FileName}";
                }
            }
            catch (Exception ex)
            {
                response.ResponseText = "Error in file uploading. Try after sometime...";
            }
            return response;
        }
    }
}
