using AppUtility.APIRequest;
using AppUtility.Helper;
using Dapper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Address;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Notify
{
    public class NotifyService : INotifyService
    {
        public readonly IDapperRepository _dapper;
        public readonly ILogger<NotifyService> _logger;
        private readonly WhatsappSetting _appSetting;
        public NotifyService(IDapperRepository dapper, ILogger<NotifyService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }


        public async Task<IResponse> SaveSMSEmailWhatsappNotification(SMSEmailWhatsappNotification req, int LoginID)
        {
            var _res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var res = await GetUserDeatilForAlert(req.UserID);
            res.OTP = req.OTP == null ? "0" : req.OTP;
            res.PhoneNumber = req.PhoneNumber == null ? res.PhoneNumber : req.PhoneNumber;
            res.WhatsappNo = req.PhoneNumber == null ? res.WhatsappNo : req.PhoneNumber;
            res.EmailID = req.EmailID == null ? res.EmailID : req.EmailID;
            res.Name = req.Name == null ? res.Name : req.Name;
            res.FormatID = req.FormatID;
            res.Password = req.Password;
            if (req.IsSms)
            {
                await NotifySMS(res);
            }
            if (req.IsEmail)
            {
                await NotifyEmail(res);
            }
            if (req.IsWhatsapp)
            {
                await NotifySocialALert(res);
            }
            return _res;
        }


        public async Task<AlertReplacementModel> GetUserDeatilForAlert(int UserID)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("LoginID", UserID, DbType.Int32);
            var res = await _dapper.GetAsync<AlertReplacementModel>("Proc_UserDetailForAlert", dbparams, CommandType.StoredProcedure);
            return res ?? new AlertReplacementModel();
        }

        #region SendSMS
        public async Task<Response> NotifySMS(AlertReplacementModel model)
        {
            var _res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            try
            {
                string sendRes = string.Empty;
                var dbparams = new DynamicParameters();
                dbparams.Add("FormatID", model.FormatID, DbType.Int32);

                string selectTemplate = @" select * from MessageTemplate(nolock) where FormatID=@FormatID
                          SELECT top(1) id, apitype,transactiontype,name, url, isactive, isdefault, isdeleted,  entryby, entrydate, modifyby,modifydate, apimethod,    
      restype,ismultipleallowed,ApiCode     
    FROM   smsapi(nolock)";
                var result = _dapper.GetMultipleAsync<MessageTemplate, SmsApi>(selectTemplate, dbparams, commandType: CommandType.Text).Result;
                var messageTemplate = (List<MessageTemplate>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var smsApi = (List<SmsApi>)result.GetType().GetProperty("Table2").GetValue(result, null);
                //ISMSAPIML ML = new APIML(_accessor, _env,_rInfo);
                var detail = smsApi.FirstOrDefault();
                if (detail != null && messageTemplate != null && messageTemplate.Count > 0)
                {
                    model.Message = messageTemplate.FirstOrDefault().SMSTemplate;
                    model.Message = GetFormatedMessage(model.Message, model);
                    var smsSetting = new SMSSetting
                    {

                        APIID = detail.id,
                        SMSID = detail.id,
                        APIMethod = detail.apimethod,
                        SenderID = messageTemplate.FirstOrDefault().SMSTemplateID,
                        IsLapu = false,
                        URL = detail.url,
                        SMS = model.Message
                    };

                    var SMSURL = new StringBuilder(smsSetting.URL);
                    if (detail.ismultipleallowed)
                    {
                        if (string.IsNullOrEmpty(model.MobileNos))
                        {
                            _res.ResponseText = "Invaild MobileMo";
                            return _res;
                        }
                        if (string.IsNullOrEmpty(model.Message))
                        {
                            _res.ResponseText = "Fill message";
                            return _res;
                        }
                        var MobileList = !string.IsNullOrEmpty(model.MobileNos) ? model.MobileNos.Split(',').ToList() : new List<string>();
                        if (MobileList == null || MobileList.Count < 1)
                        {
                            _res.ResponseText = "Invaild MobileMo";
                            return _res;
                        }
                        var _l = MobileList.LastOrDefault();
                        if (string.IsNullOrEmpty(_l))
                        {
                            MobileList.RemoveAt(MobileList.Count - 1);
                        }
                        smsSetting.MobileNos = string.Join(",", MobileList);
                        foreach (var item in MobileList)
                        {

                            SMSURL.Clear();
                            SMSURL.Append(smsSetting.URL);
                            SMSURL.Replace("{SENDERID}", smsSetting.SenderID ?? "");
                            SMSURL.Replace("{TO}", item);
                            SMSURL.Replace("{MESSAGE}", smsSetting.SMS);

                            var p = new NotifyModel
                            {
                                Method = smsSetting.APIMethod,
                                ApiUrl = SMSURL.ToString(),
                                Message = smsSetting.SMS,
                                CommunicationMode = (int)CommunicationMode.SMS,
                                ApiType = detail.apitype,
                                UserID = model.UserID,
                                EncryptedData = "",
                                EmailConfiguration = "",
                                Subject = "",
                                SendTo = model.PhoneNumber,
                                APIID = smsSetting.APIID
                            };
                            SaveNotify(p);
                        }
                    }
                    else
                    {
                        SMSURL.Replace("{SENDERID}", smsSetting.SenderID ?? "");
                        SMSURL.Replace("{TO}", model.PhoneNumber);
                        SMSURL.Replace("{MESSAGE}", smsSetting.SMS);
                        var p = new NotifyModel()
                        {
                            Method = smsSetting.APIMethod,
                            ApiUrl = SMSURL.ToString(),
                            Message = smsSetting.SMS,
                            CommunicationMode = (int)CommunicationMode.SMS,
                            ApiType = 0,
                            UserID = model.UserID,
                            EncryptedData = HashEncryption.O.Encrypt(model.OTP ?? "0"),
                            EmailConfiguration = "",
                            Subject = "",
                            SendTo = model.PhoneNumber,
                            APIID = smsSetting.APIID
                        };
                        SaveNotify(p);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            _res.StatusCode = ResponseStatus.Success;
            return _res;
        }


        #endregion
        #region SendEmail
        public async Task<Response> NotifyEmail(AlertReplacementModel param)
        {
            var _res = new Response
            {
                StatusCode = ResponseStatus.Success,
                ResponseText = "OTP has been sent to your registered Email"
            };
            try
            {
                bool IsSent = false;

                string EmailBody = string.Empty, ForSaveEmailBody = string.Empty;
                var dbparams = new DynamicParameters();
                dbparams.Add("LoginID", param.UserID, DbType.Int32);
                dbparams.Add("FormatID", param.FormatID, DbType.Int32);
                string selectTemplate = @" select * from MessageTemplate(nolock) where FormatID=@FormatID
                          select ID,FromEmail, Password,HostName,SmtpUserName,Port,WID,EntryByLT,EntryBy,EntryDate,ModifyByLT,ModifyBy,ModifyDate,  
                                        IsActive, IsEmailVerified,IsSSL ,MailUserID,IsDefault,WID          
                            from emailsetting where   isactive=1 order by ID   ";
                var result = _dapper.GetMultipleAsync<MessageTemplate, EmailApi>(selectTemplate, dbparams, commandType: CommandType.Text).Result;
                var emailTemplate = (List<MessageTemplate>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var emailApi = (List<EmailApi>)result.GetType().GetProperty("Table2").GetValue(result, null);
                var mailSetting = emailTemplate.FirstOrDefault();
                var email = emailApi.FirstOrDefault();
                if (mailSetting != null)
                {
                    bool IsNoTemplate = true;
                    if (string.IsNullOrEmpty(mailSetting.EmailTemplate))
                    {
                        ForSaveEmailBody = EmailBody = "No Template Found";
                        IsNoTemplate = false;
                    }
                    if (IsNoTemplate)
                    {
                        if (string.IsNullOrEmpty(email.FromEmail))
                        {
                            ForSaveEmailBody = EmailBody = "No Email Found";
                        }
                        EmailBody = GetFormatedMessage(mailSetting.EmailTemplate, param);
                        //param.OTP = "******";
                        ForSaveEmailBody = GetFormatedMessageForSaving(mailSetting.EmailTemplate, param);

                        if (!string.IsNullOrEmpty(email.FromEmail))
                        {
                            string emailconfiguration = JsonSerializer.Serialize(email);
                            var p = new NotifyModel()
                            {
                                Method = "",
                                ApiUrl = "",
                                Message = EmailBody,
                                CommunicationMode = (int)CommunicationMode.Email,
                                ApiType = 0,
                                UserID = param.UserID,
                                EncryptedData = HashEncryption.O.Encrypt(param.OTP ?? "0"),
                                EmailConfiguration = emailconfiguration,
                                Subject = mailSetting.EmailSubject,
                                SendTo = param.EmailID,
                                APIID = 0,
                                ImageUrl = "https://lapurobotics.com/img/logo-dark.png"//DOCType.BannerImagePathSuffix + "Emailimage.png",
                            };
                            SaveNotify(p);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return _res;
        }


        #endregion
        #region SocialAlert
        public async Task<Response> NotifySocialALert(AlertReplacementModel param)
        {

            var _res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            try
            {
                //IProcedure _p = new ProcGetMasterSocialType(_dal);
                //string resp = (ResponseStatus)_p.Call();
                var ActivatedType = "1".Split(",");
                string sendTo = string.Empty;
                for (int i = 0; i < ActivatedType.Count(); i++)
                {
                    switch (int.Parse(ActivatedType[i]))// switch (i + 1)
                    {
                        case 1:
                            sendTo = param.WhatsappNo ?? param.PhoneNumber;
                            break;
                        case 2:
                            sendTo = param.TelegramNo;
                            break;
                        case 3:
                            sendTo = param.HangoutNo;
                            break;
                    }
                    param.SocialAlertType = int.Parse(ActivatedType[i]);
                    var dbparams = new DynamicParameters();
                    dbparams.Add("FormatID", (int)param.FormatID, DbType.Int32);
                    string selectTemplate = @" select * from MessageTemplate(nolock) where FormatID=@FormatID
                         SELECT 1,id, apitype,transactiontype,name, url, isactive, isdefault, isdeleted,  entryby, entrydate, modifyby,modifydate, apimethod,      
      restype,ismultipleallowed,ApiCode       
    FROM   smsapi(nolock)  where id=2  ";

                    var result = _dapper.GetMultipleAsync<SocialAlertFormat, SmsApi>(selectTemplate, dbparams, CommandType.Text).Result;
                    var socialAlertFormat = (List<SocialAlertFormat>)result.GetType().GetProperty("Table1").GetValue(result, null);
                    var smsapi = (List<SmsApi>)result.GetType().GetProperty("Table2").GetValue(result, null);

                    if (smsapi == null && smsapi.Count < 1)
                    {
                        return _res;
                    }
                    if (socialAlertFormat != null && socialAlertFormat.Count > 0)
                    {
                        var NotificationDetail = socialAlertFormat.FirstOrDefault();
                        string formatedMessage = GetFormatedMessage(!string.IsNullOrEmpty(param.Message) ? param.Message : NotificationDetail.WhatsappTemplate, param);
                        if (!string.IsNullOrEmpty(NotificationDetail.WhatsappTitle))
                        {
                            NotificationDetail.WhatsappTitle = GetFormatedMessage(NotificationDetail.WhatsappTitle, param);
                        }
                        if (!string.IsNullOrEmpty(NotificationDetail.WhatsappFooter))
                        {
                            NotificationDetail.WhatsappFooter = GetFormatedMessage(NotificationDetail.WhatsappFooter, param);
                        }
                        //param.OTP = "******";
                        string formatedMessageSave = GetFormatedMessageForSaving(!string.IsNullOrEmpty(param.Message) ? param.Message : NotificationDetail.WhatsappTemplate, param);
                        var detail = smsapi.FirstOrDefault();
                        if (!string.IsNullOrEmpty(sendTo))
                        {
                            var p = new NotifyModel()
                            {
                                Method = detail.apimethod,
                                ApiUrl = detail.url,
                                Message = formatedMessage,
                                CommunicationMode = (int)CommunicationMode.Social,
                                ApiType = detail.apitype,
                                UserID = param.UserID,
                                EncryptedData = HashEncryption.O.Encrypt(param.OTP ?? "0"),
                                EmailConfiguration = String.Empty,
                                Subject = String.Empty,
                                SendTo = param.WhatsappNo,
                                SendFrom = NotificationDetail.ScanNo,
                                APIID = detail.id,
                                ButtonJson = NotificationDetail.ButtonJson,
                                WhatsappTitle = NotificationDetail.WhatsappTitle,
                                WhatsappFooter = NotificationDetail.WhatsappFooter,
                                ImageUrl = param.URL,
                                WhatsappTitleType = NotificationDetail.WhatsappTitleType,
                            };
                            SaveNotify(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return _res;
        }
        #endregion

        public async Task<Response> SaveNotify(NotifyModel ss)
        {
            var _res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var dbparams = new DynamicParameters();
            try
            {
                dbparams.Add("CommunicationMode", ss.CommunicationMode, DbType.Int32);
                dbparams.Add("Message", ss.Message, DbType.String);
                dbparams.Add("ApiType", ss.ApiType, DbType.Int32);
                dbparams.Add("ApiUrl", ss.ApiUrl, DbType.String);
                dbparams.Add("Method", ss.Method, DbType.String);
                dbparams.Add("UserID", ss.UserID, DbType.Int32);
                dbparams.Add("EncryptedData", ss.EncryptedData, DbType.String);
                dbparams.Add("Subject", ss.Subject ?? "", DbType.String);
                dbparams.Add("EmailConfiguration", ss.EmailConfiguration ?? "", DbType.String);
                dbparams.Add("SendTo", ss.SendTo ?? "", DbType.String);
                dbparams.Add("APIID", ss.APIID, DbType.Int32);
                dbparams.Add("ButtonJson", ss.ButtonJson, DbType.String);
                dbparams.Add("WhatsappTitle", ss.WhatsappTitle, DbType.String);
                dbparams.Add("WhatsappFooter", ss.WhatsappFooter, DbType.String);
                dbparams.Add("ImageUrl", ss.ImageUrl, DbType.String);
                dbparams.Add("SendFrom", ss.SendFrom, DbType.String);
                dbparams.Add("WhatsappTitleType", ss.WhatsappTitleType, DbType.String);
                string sqlQuery = @"insert into Notify(APIID,CommunicationMode,[Subject],[Message],ApiType,SendTo,  
            BCC,EmailConfiguration,ApiUrl,Method,EncryptedData,IsProcced,CreatedOn,CreatedBY,
            ModifyOn,ButtonJson,WhatsappTitle,WhatsappFooter,ImageUrl,SendFrom,WhatsappTitleType)              
            values(@APIID,@CommunicationMode,@Subject,@Message,@ApiType,@SendTo,'',@EmailConfiguration,  
            @ApiUrl,@Method,@EncryptedData,0,GETDATE(),@UserID,'',@ButtonJson,@WhatsappTitle,@WhatsappFooter,
            @ImageUrl,@SendFrom,@WhatsappTitleType)";
                int i = -5;
                i = await _dapper.ExecuteAsync(sqlQuery, dbparams, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    _res.StatusCode = ResponseStatus.Success;
                    _res.ResponseText = ResponseStatus.Success.ToString();
                    GetNotify();
                }
                else
                {
                    _res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {

            }
            return _res;
        }

        private string GetFormatedMessage(string Template, AlertReplacementModel Replacements)
        {
            StringBuilder sb = new StringBuilder(Template);
            sb.Replace("{Name}", Replacements.Name);
            sb.Replace("{FromUserName}", Replacements.LoginUserName);
            sb.Replace("{FromUserMobile}", Replacements.PhoneNumber);
            sb.Replace("{Amount}", Convert.ToString(Replacements.Amount));
            sb.Replace("{Company}", Replacements.Company);
            sb.Replace("{CompanyName}", Replacements.Company);
            sb.Replace("{CompanyDomain}", Replacements.CompanyDomain);
            sb.Replace("{CompanyAddress}", Replacements.CompanyAddress);
            sb.Replace("{BrandName}", Replacements.BrandName);
            sb.Replace("{SupportNumber}", Replacements.SupportNumber);
            sb.Replace("{SupportEmail}", Replacements.SupportEmail);
            sb.Replace("{OTP}", Replacements.OTP);
            sb.Replace("{LoginID}", Replacements.PhoneNumber.ToString());
            sb.Replace("{LiveID}", Replacements.LiveID);
            sb.Replace("{TransactionID}", Replacements.TransactionID);
            sb.Replace("{Password}", Replacements.Password);
            return Convert.ToString(sb);
        }
        private string GetFormatedMessageForSaving(string Template, AlertReplacementModel Replacements)
        {
            string star = "******";
            StringBuilder sb = new StringBuilder(Template);
            sb.Replace("{FromUserName}", Replacements.LoginUserName);
            sb.Replace("{FromUserMobile}", Replacements.PhoneNumber);
            sb.Replace("{Amount}", Convert.ToString(Replacements.Amount));
            sb.Replace("{Company}", Replacements.Company);
            sb.Replace("{CompanyName}", Replacements.Company);
            sb.Replace("{CompanyDomain}", Replacements.CompanyDomain);
            sb.Replace("{CompanyAddress}", Replacements.CompanyAddress);
            sb.Replace("{BrandName}", Replacements.BrandName);
            sb.Replace("{SupportNumber}", Replacements.SupportNumber);
            sb.Replace("{SupportEmail}", Replacements.SupportEmail);
            sb.Replace("{OTP}", star);
            sb.Replace("{LoginID}", Replacements.LoginID.ToString());
            sb.Replace("{Password}", star);
            sb.Replace("{PinPassword}", star);
            sb.Replace("{LiveID}", Replacements.LiveID);
            sb.Replace("{TransactionID}", Replacements.TransactionID);
            sb.Replace("{RejectReason}", Replacements.KycRejectReason);
            sb.Replace("{DATETIME}", Replacements.DATETIME);

            return Convert.ToString(sb);
        }

        #region NotifyModel Data
        public async Task<IResponse<IEnumerable<NotifyModel>>> GetNotify()
        {
            var res = new Response<IEnumerable<NotifyModel>>();
            try
            {
                string query = @"select ID,CommunicationMode,SendTo,BCC,Subject,Message,ApiType,EmailConfiguration,ApiUrl,Method,EncryptedData,  
                                    IsProcced,CreatedOn,CreatedBY,ModifyOn,ButtonJson,WhatsappFooter,WhatsappTitle,ImageUrl,WhatsappTitleType,SendFrom
                                    from notify where IsProcced = 0   ";
                var dbparams = new DynamicParameters();
                res.Result = await _dapper.GetAllAsync<NotifyModel>(query, dbparams, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
                if (res != null && res.Result.ToList().Count > 0)
                {
                    foreach (var item in res.Result)
                    {
                        if (item.CommunicationMode == (int)CommunicationMode.SMS)
                        {
                            SendSms(item);
                        }
                        else if (item.CommunicationMode == (int)CommunicationMode.Email)
                        {
                            SendEmail(item);
                        }
                        else if (item.CommunicationMode == (int)CommunicationMode.Social)
                        {
                            SendSocialAlertWhatsapp(item);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        private async Task SendSms(NotifyModel item)
        {
            string sendRes = string.Empty;
            try
            {
                if (item.ApiType != 2)
                {
                    var p = new SendSMSRequest()
                    {
                        APIMethod = item.Method,
                        SmsURL = item.ApiUrl,
                        IsLapu = false
                    };
                    sendRes = CallSendSMSAPI(p).Result;
                }
                var _Response = new SMSResponse
                {
                    ReqURL = item.ApiUrl,
                    Response = item.ApiType != 2 ? sendRes : "Only Save Sms",
                    ResponseID = "",
                    Status = SMSResponseTYPE.SEND,
                    SMSID = item.APIID,
                    MobileNo = item.SendTo,
                    TransactionID = "",
                    SMS = item.Message,
                    NotifyID = item.ID
                };
                SaveSMSResponse(_Response);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task SendEmail(NotifyModel item)
        {
            try
            {


                EmailSettingswithFormat mailSetting = JsonSerializer.Deserialize<EmailSettingswithFormat>(item.EmailConfiguration);




                var emailresponse = SendEMailAsync(mailSetting, item.SendTo, null, item.Subject, item.Message, 0, "LogoURL", true).Result;

                SendEmail sendEmail = new SendEmail
                {
                    From = mailSetting.FromEmail,
                    Body = item.Message,
                    Recipients = item.SendTo,
                    Subject = item.Subject,
                    IsSent = true,
                    NotifyID = item.ID,
                    Response = emailresponse ? "Success" : "Failed"
                };
                SaveEMailResponse(sendEmail);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task SendSocialAlertWhatsapp(NotifyModel item)
        {
            var response = new ResponseStatus();
            var SMSResponse = new SMSResponse();
            try
            {
                string TextOrImage = "TEXT";
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    TextOrImage = "IMAGE";
                }
                if (item.ApiUrl != null && item.ApiType != 2)
                {
                    string sendRes = string.Empty;
                    var SMSURL = new StringBuilder(item.ApiUrl);
                    if (!string.IsNullOrEmpty(item.SendTo))
                    {
                        if (string.IsNullOrEmpty(item.ButtonJson))
                        {
                            string SenderID = "";
                            SMSURL.Replace("{SENDERID}", SenderID ?? "");
                            SMSURL.Replace("{TO}", item.SendTo);
                            SMSURL.Replace("{MESSAGE}", item.Message);
                            SMSURL.Replace("{SCANNO}", item.SendFrom ?? string.Empty);
                            SMSURL.Replace("{MESSAGETYPE}", TextOrImage);
                            SMSURL.Replace("{URL}", item.ImageUrl ?? string.Empty);
                            var _p = new SendSMSRequest()
                            {
                                APIMethod = item.Method,
                                SmsURL = SMSURL.ToString(),
                                IsLapu = false
                            };
                            sendRes = CallSendSMSAPI(_p).Result;
                        }
                        else
                        {
                            List<ButtonAertHub> buttons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ButtonAertHub>>(item.ButtonJson);
                            List<dynamic> b = new List<dynamic>();
                            buttons.ForEach(x =>
                            {
                                if (x.btn_type?.ToUpper() == "CALL")
                                {
                                    b.Add(new
                                    {
                                        x.btn_type,
                                        x.display_txt,
                                        x.call
                                    });
                                }
                                else if (x.btn_type?.ToUpper() == "URL")
                                {
                                    b.Add(new
                                    {
                                        x.btn_type,
                                        x.display_txt,
                                        x.url
                                    });
                                }
                            });
                            if (!string.IsNullOrEmpty(item.SendTo))
                            {
                                item.SendTo = item.SendTo.Length == 10 ? "91" + item.SendTo : item.SendTo;
                            }
                            var objAlertHub = new WhatsappAPIAlertHubButtons
                            {
                                from = item.SendFrom,
                                // from = "918604695079",
                                titletype = item.WhatsappTitleType ?? "TEXT",
                                title = item.WhatsappTitle,
                                footer = item.WhatsappFooter,
                                jid = item.SendTo,
                                messagetype = "BTNResponseText",
                                content = item.Message,
                                requestid = Utility.O.GenrateRandom(10, true),
                                buttons = b,
                            };
                            sendRes = AlertHub_SendSessionMessageButton(objAlertHub).Result;
                            SMSURL.Clear();
                            SMSURL.Append(objAlertHub.content);
                        }
                    }

                    var _Response = new SMSResponse
                    {
                        ReqURL = SMSURL.ToString(),
                        Response = sendRes,
                        ResponseID = "",
                        Status = SMSResponseTYPE.SEND,
                        SMSID = item.APIID,
                        MobileNo = item.SendTo,
                        TransactionID = "",
                        SMS = item.Message,
                        NotifyID = item.ID
                    };
                    SaveSMSResponse(_Response);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<string> AlertHub_SendSessionMessageButton(WhatsappAPIAlertHubButtons _ObjAlertHub)
        {
            string resp = string.Empty;
            try
            {
                _ObjAlertHub.apiusername = _appSetting.AlertHub.UserName;
                _ObjAlertHub.apipassword = _appSetting.AlertHub.Password;
                resp = await AppWebRequest.O.PostJsonDataUsingHWRTLS(_appSetting.AlertHub.BaseUrl, _ObjAlertHub, null).ConfigureAwait(false);
                _ObjAlertHub.content = _appSetting.AlertHub.BaseUrl;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }


        #region Email and Sms Response Save
        private async Task SaveSMSResponse(SMSResponse Response)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("SMSID", Response.SMSID, DbType.Int32);
                dbparams.Add("MobileNo", Response.MobileNo, DbType.String);
                dbparams.Add("SMS", Response.SMS, DbType.String);
                dbparams.Add("Status", Response.Status, DbType.String);
                dbparams.Add("TransactionID", Response.TransactionID, DbType.String);
                dbparams.Add("Response", Response.Response, DbType.String);
                dbparams.Add("ResponseID", Response.ResponseID, DbType.String);
                dbparams.Add("ReqURL", Response.ReqURL, DbType.String);
                dbparams.Add("SocialAlertType", Response.SocialAlertType, DbType.String);
                dbparams.Add("NotifyID", Response.NotifyID, DbType.Int32);
                string query = @"insert into SendSMS(APIID, MobileNO,[Message],[Status], TransactionID, ResponseID, Response, EntryDate, IsRead,
                                          ModifyDate,  Req, SocialAlertType)
                                      values(@SMSID, @MobileNo, @SMS, @Status, dbo.fn_TransactionID(), @ResponseID, Replace(@Response, 'login.js', ''), getDate(),0,
                                        getDate(),  @ReqURL, @SocialAlertType)
                                        update notify set IsProcced = 1, ModifyOn = GETDATE() where ID = @NotifyID";
                await _dapper.ExecuteAsync(query, dbparams, CommandType.Text);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task SaveEMailResponse(SendEmail sendEmail)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("From", sendEmail.From, DbType.String);
                dbparams.Add("Recipients", sendEmail.Recipients, DbType.String);
                dbparams.Add("Subject", sendEmail.Subject ?? "", DbType.String);
                dbparams.Add("Body", sendEmail.Body, DbType.String);
                dbparams.Add("IsSent", sendEmail.IsSent, DbType.Boolean);
                dbparams.Add("NotifyID", sendEmail.NotifyID, DbType.Int32);
                dbparams.Add("Response", sendEmail.Response, DbType.String);
                string query = @" INSERT INTO SendEmailLog ([From],Recipients,[Subject],Body,IsSent,EntryDate,ModifyDate,Response)VALUES(@From,@Recipients,@Subject,@Body,@IsSent,getdate(),GETDATE(),@Response); 
                      update notify set IsProcced=1,ModifyOn=GETDATE() where ID=@NotifyID";
                await _dapper.ExecuteAsync(query, dbparams, CommandType.Text);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion



        #endregion
        #region SendSMS Service

        public void SendUserReg(string LoginID, string Password, string MobileNo, string EmailID, int WID, string Logo)
        {
            try
            {
                DataTable Tp_ReplaceKeywords = new DataTable();
                Tp_ReplaceKeywords.Columns.Add("Keyword", typeof(string));
                Tp_ReplaceKeywords.Columns.Add("ReplaceValue", typeof(string));
                Tp_ReplaceKeywords.Rows.Add(AddKeyAndValue(MessageTemplateKeywords.LoginID, LoginID));
                SendSMS(Tp_ReplaceKeywords, MobileNo, EmailID, WID, true, "User Registration", (int)MessageFormat.Registration, Logo);
            }
            catch (Exception ex)
            {
                //var _ = new ProcPageErrorLog(_dal).Call(errorLog);
            }

        }
        public void SendSMS(DataTable Tp_ReplaceKeywords, string MobileNo, string EmailID, int WID, bool WithMail, string MailSub, int FormatType, string Logo)
        {
            if (Tp_ReplaceKeywords == null)
            {
                Tp_ReplaceKeywords = new DataTable();
                Tp_ReplaceKeywords.Columns.Add("Keyword", typeof(string));
                Tp_ReplaceKeywords.Columns.Add("ReplaceValue", typeof(string));
                Tp_ReplaceKeywords.Rows.Add(AddKeyAndValue(MessageTemplateKeywords.LoginID, "0"));
            }
            var procSendSMS = new SMSSendREQ
            {
                FormatType = FormatType,
                MobileNo = MobileNo,
                Tp_ReplaceKeywords = Tp_ReplaceKeywords,

            };
            // SMSSendResp smsResponse = (SMSSendResp)SendSMS(procSendSMS);
            //if (WithMail)
            //{
            //    if (!string.IsNullOrEmpty(smsResponse.SMS) && !string.IsNullOrEmpty(EmailID))
            //    {
            //        IEmailML emailManager = new EmailML(_dal);
            //        emailManager.SendMail(EmailID, null, MailSub, smsResponse.SMS, WID, Logo);
            //    }
            //}
        }


        private DataRow AddKeyAndValue(object loginID1, string loginID2)
        {
            throw new NotImplementedException();
        }



        public async Task<string> CallSendSMSAPI(SendSMSRequest _req)
        {
            string ApiResp = "";
            try
            {
                if (_req.APIMethod == "GET")
                {
                    ApiResp = await AppWebRequest.O.CallUsingHttpWebRequest_GET(_req.SmsURL);
                    _req.IsSend = true;
                }
                if (_req.APIMethod == "POST")
                {

                }
            }
            catch (Exception ex)
            {
                ApiResp = "Exception Occured! " + ex.Message;
            }
            return ApiResp;
        }
        #endregion
        public async Task<bool> SendEMailAsync(EmailSettingswithFormat setting, string ToEmail, List<string> bccList, string Subject, string Body, int WID, string Logo, bool IsHTML = true, string MailFooter = "")
        {
            // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            bool IsSent = false;
            if (setting.FromEmail != null && setting.Port != 0 && setting.Password != null && setting.HostName != null)
            {
                string htmlMailTemplate = @"<html><head><title></title><meta charset='utf - 8'><meta name='viewport' content='width = device - width,initial - scale = 1'></head><body><link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css'><div style='margin:0;text-align:center;font-family:Karla,sans-serif;color:#092147;background-color:#eaf0f8'><div style='width:624px;display:inline-block;border:1px solid #f8f8f9;background-color:#f8f8f9'><div style='padding:15px'><div class='d-flex align-items-center'><div><img alt='vauld' src='https://shop99.co.in/assets/images/layout-2/logo/logo.png' style='width:137px' class='CToWUd a6T mr-3' data-bit='iit' tabindex='0'></div><div><h3>Shop99</h3><p class='mb-1' style='font-size:13px'>Sales@Shop99.Co.In , Support@Shop99.Co.In</p><p class='mb-1' style='font-size:13px'>+91-8920114845</p></div></div></div><div style='background-color:#fff;padding:0 48px;text-align:left'><div style='margin-top:20px;font-size:15px;color:#092147;line-height:1.5;display:inline-block;width:100%'>{BODY}</div><div style='height:2px;background-color:#f6f9fd;margin:20px 0'></div></div><div style='text-align:center;margin-top:20px'><div style='margin-top:20px;font-size:12px;color:#09214799'>Shop with Shop99.<br>Buy and sell Product.</div><div style='margin:28px 0 33px;color:#09214799;font-size:10px;line-height:1.5'>© 2022 Shop99. All rights reserved</div></div></div></div></body></html> ";

                if (IsHTML)
                {
                    Body = htmlMailTemplate.Replace("{BODY}", Body).ToString();
                }
                try
                {

                    // MailMessage mailMessage = new System.Net.Mail.MailMessage(setting.FromEmail, ToEmail.Trim(),setting.Password, "Hello Word");
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(setting.FromEmail),
                        Subject = Subject,
                        Body = Body,
                        IsBodyHtml = IsHTML
                    };
                    ToEmail = string.IsNullOrEmpty(ToEmail) ? setting.FromEmail : ToEmail;
                    mailMessage.To.Add(ToEmail);
                    if (bccList != null)
                    {
                        foreach (string bcc in bccList)
                        {
                            if (bcc.Contains("@") && bcc.Contains(".") && bcc.Length <= 255)
                            {
                                mailMessage.Bcc.Add(bcc.ToLower());
                            }
                        }
                    }
                    SmtpClient smtpClient = new SmtpClient(setting.HostName, setting.Port)
                    {
                        Credentials = new NetworkCredential(setting.FromEmail, setting.Password)
                    };
                    if (setting.IsSSL)
                    {
                        smtpClient.EnableSsl = setting.IsSSL;
                    }
                    try
                    {
                        smtpClient.Send(mailMessage);
                        IsSent = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
            return IsSent;
        }




    }
}
