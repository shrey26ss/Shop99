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
                dbparams.Add("LoginID", model.UserID, DbType.Int32);
                dbparams.Add("FormatID", model.FormatID, DbType.Int32);
                var result = _dapper.GetMultipleAsync<MessageTemplate, SmsApi>("[proc_GetSMSFormatSetting]", dbparams, commandType: CommandType.StoredProcedure).Result;
                var messageTemplate = (List<MessageTemplate>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var smsApi = (List<SmsApi>)result.GetType().GetProperty("Table2").GetValue(result, null);
                //ISMSAPIML ML = new APIML(_accessor, _env,_rInfo);
                var detail = smsApi.FirstOrDefault();
                if (detail != null && messageTemplate != null && messageTemplate.Count > 0 && messageTemplate.FirstOrDefault().IsSMSEnable)
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
                                SendTo = model.UserMobileNo,
                                APIID = smsSetting.APIID
                            };
                            SaveNotify(p);
                        }
                    }
                    else
                    {
                        SMSURL.Replace("{SENDERID}", smsSetting.SenderID ?? "");
                        SMSURL.Replace("{TO}", model.UserMobileNo);
                        SMSURL.Replace("{MESSAGE}", smsSetting.SMS);
                        var p = new NotifyModel()
                        {
                            Method = smsSetting.APIMethod,
                            ApiUrl = SMSURL.ToString(),
                            Message = smsSetting.SMS,
                            CommunicationMode = (int)CommunicationMode.SMS,
                            ApiType = 0,
                            UserID = model.UserID,
                            EncryptedData = HashEncryption.O.Encrypt(model.OTP),
                            EmailConfiguration = "",
                            Subject = "",
                            SendTo = model.UserMobileNo,
                            APIID = smsSetting.APIID
                        };
                        SaveNotify(p);
                    }
                }
            }
            catch (Exception ex)
            {
               // _dapper.SaveDBError(ex.Message, this.GetType().Name, nameof(NotifySMS));
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
               
                var result = _dapper.GetMultipleAsync<MessageTemplate, EmailApi>("[proc_GetEmailFormatSetting]", dbparams, commandType: CommandType.StoredProcedure).Result;
                var emailTemplate = (List<MessageTemplate>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var emailApi = (List<EmailApi>)result.GetType().GetProperty("Table2").GetValue(result, null);
                var mailSetting = emailTemplate.FirstOrDefault();
                var email = emailApi.FirstOrDefault();
                if (mailSetting != null && mailSetting.IsEmailEnable && mailSetting.IsEmailEnable)
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
                                    SendTo = param.UserEmailID,
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
                            sendTo = param.WhatsappNo;
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
                    dbparams.Add("LoginID", param.UserID, DbType.Int32);
                    dbparams.Add("FormatID", param.FormatID, DbType.Int32);
                    dbparams.Add("SocialAlertType", param.SocialAlertType, DbType.Int32);

                    var result = _dapper.GetMultipleAsync<SocialAlertFormat, SmsApi>("[Proc_GetSocialAlertFormat]", dbparams, commandType: CommandType.StoredProcedure).Result;
                    var socialAlertFormat = (List<SocialAlertFormat>)result.GetType().GetProperty("Table1").GetValue(result, null);
                    var smsapi = (List<SmsApi>)result.GetType().GetProperty("Table2").GetValue(result, null);

                    if (smsapi == null && smsapi.Count < 1)
                    {
                        return _res;
                    }
                    if (socialAlertFormat != null && socialAlertFormat.Count > 0)
                    {
                        var NotificationDetail = socialAlertFormat.FirstOrDefault();
                        if (NotificationDetail.IsSocialAlert)// check if SocialAlert Enable
                        {
                            string formatedMessage = GetFormatedMessage(!string.IsNullOrEmpty(param.Message) ? param.Message : NotificationDetail.SocialAlertTemplate, param);
                            if (!string.IsNullOrEmpty(NotificationDetail.WhatsappTitle))
                            {
                                NotificationDetail.WhatsappTitle = GetFormatedMessage(NotificationDetail.WhatsappTitle, param);
                            }
                            if (!string.IsNullOrEmpty(NotificationDetail.WhatsappFooter))
                            {
                                NotificationDetail.WhatsappFooter = GetFormatedMessage(NotificationDetail.WhatsappFooter, param);
                            }
                            //param.OTP = "******";
                            string formatedMessageSave = GetFormatedMessageForSaving(!string.IsNullOrEmpty(param.Message) ? param.Message : NotificationDetail.SocialAlertTemplate, param);
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
                dbparams.Add("WID", ss.WID, DbType.Int32);
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
                string sqlQuery = @"insert into tbl_notify(WID,APIID,CommunicationMode,[Subject],[Message],ApiType,SendTo,  
            BCC,EmailConfiguration,ApiUrl,Method,EncryptedData,IsProcced,CreatedOn,CreatedBY,
            ModifyOn,ButtonJson,WhatsappTitle,WhatsappFooter,ImageUrl,SendFrom,WhatsappTitleType)              
            values(@WID,@APIID,@CommunicationMode,@Subject,@Message,@ApiType,@SendTo,'',@EmailConfiguration,  
            @ApiUrl,@Method,@EncryptedData,0,GETDATE(),@UserID,'',@ButtonJson,@WhatsappTitle,@WhatsappFooter,
            @ImageUrl,@SendFrom,@WhatsappTitleType)      d";
                int i = -5;
                i = await _dapper.ExecuteAsync(sqlQuery, dbparams, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    _res.StatusCode = ResponseStatus.Success;
                    _res.ResponseText = ResponseStatus.Success.ToString();
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

            sb.Replace("{FromUserName}", Replacements.LoginUserName);
            sb.Replace("{FromUserMobile}", Replacements.LoginMobileNo);
            sb.Replace("{FromUserID}", Replacements.LoginPrefix + Replacements.LoginID.ToString());
            sb.Replace("{ToUserMobile}", Replacements.UserMobileNo);
            sb.Replace("{ToUserID}", Replacements.UserPrefix + Replacements.UserID.ToString());
            sb.Replace("{ToUserName}", Replacements.UserName);
            sb.Replace("{UserName}", Replacements.UserName);
            sb.Replace("{Mobile}", Replacements.UserMobileNo);
            sb.Replace("{UserMobile}", Replacements.UserMobileNo);
            sb.Replace("{Amount}", Convert.ToString(Replacements.Amount));
            sb.Replace("{BalanceAmount}", Convert.ToString(Replacements.BalanceAmount));
            sb.Replace("{UserBalanceAmount}", Convert.ToString(Replacements.UserCurrentBalance));
            sb.Replace("{LoginBalanceAmount}", Convert.ToString(Replacements.LoginCurrentBalance));
            sb.Replace("{Operator}", Replacements.Operator);
            sb.Replace("{OperatorName}", Replacements.Operator);
            sb.Replace("{Company}", Replacements.Company);
            sb.Replace("{CompanyName}", Replacements.Company);
            sb.Replace("{CompanyDomain}", Replacements.CompanyDomain);
            sb.Replace("{CompanyAddress}", Replacements.CompanyAddress);
            sb.Replace("{BrandName}", Replacements.BrandName);
            sb.Replace("{OutletName}", Replacements.OutletName);
            sb.Replace("{SupportNumber}", Replacements.SupportNumber);
            sb.Replace("{SupportEmail}", Replacements.SupportEmail);
            sb.Replace("{AccountNumber}", Replacements.AccountNo);
            sb.Replace("{AccountsContactNo}", Replacements.AccountsContactNo);
            sb.Replace("{AccountEmail}", Replacements.AccountEmail);
            sb.Replace("{OTP}", Replacements.OTP);
            sb.Replace("{LoginID}", Replacements.LoginID.ToString());
            sb.Replace("{Password}", Replacements.Password);
            sb.Replace("{PinPassword}", Replacements.PinPassword);
            sb.Replace("{AccountNo}", Replacements.AccountNo);
            sb.Replace("{LiveID}", Replacements.LiveID);
            sb.Replace("{TID}", Convert.ToString(Replacements.TID));
            sb.Replace("{TransactionID}", Replacements.TransactionID);
            sb.Replace("{BankRequestStatus}", Replacements.RequestStatus);
            sb.Replace("{DATETIME}", Replacements.DATETIME);
            sb.Replace("{Duration}", Replacements.Duration);
            //sb.Replace(MessageTemplateKeywords.AccountNumber, Replacements.AccountNumber);
            return Convert.ToString(sb);
        }
        private string GetFormatedMessageForSaving(string Template, AlertReplacementModel Replacements)
        {
            string star = "******";
            StringBuilder sb = new StringBuilder(Template);

            sb.Replace("{FromUserName}", Replacements.LoginUserName);
            sb.Replace("{FromUserMobile}", Replacements.LoginMobileNo);
            sb.Replace("{FromUserID}", Replacements.LoginPrefix + Replacements.LoginID.ToString());
            sb.Replace("{ToUserMobile}", Replacements.UserMobileNo);
            sb.Replace("{ToUserID}", Replacements.UserPrefix + Replacements.UserID.ToString());
            sb.Replace("{ToUserName}", Replacements.UserName);
            sb.Replace("{UserName}", Replacements.UserName);
            sb.Replace("{Mobile}", Replacements.UserMobileNo);
            sb.Replace("{UserMobile}", Replacements.UserMobileNo);
            sb.Replace("{Amount}", Convert.ToString(Replacements.Amount));
            sb.Replace("{BalanceAmount}", Convert.ToString(Replacements.BalanceAmount));
            sb.Replace("{UserBalanceAmount}", Convert.ToString(Replacements.UserCurrentBalance));
            sb.Replace("{LoginBalanceAmount}", Convert.ToString(Replacements.LoginCurrentBalance));
            sb.Replace("{Operator}", Replacements.Operator);
            sb.Replace("{OperatorName}", Replacements.Operator);
            sb.Replace("{Company}", Replacements.Company);
            sb.Replace("{CompanyName}", Replacements.Company);
            sb.Replace("{CompanyDomain}", Replacements.CompanyDomain);
            sb.Replace("{CompanyAddress}", Replacements.CompanyAddress);
            sb.Replace("{BrandName}", Replacements.BrandName);
            sb.Replace("{OutletName}", Replacements.OutletName);
            sb.Replace("{SupportNumber}", Replacements.SupportNumber);
            sb.Replace("{SupportEmail}", Replacements.SupportEmail);
            sb.Replace("{AccountNumber}", Replacements.AccountNo);
            sb.Replace("{AccountsContactNo}", Replacements.AccountsContactNo);
            sb.Replace("{AccountEmail}", Replacements.AccountEmail);
            sb.Replace("{OTP}", star);
            sb.Replace("{LoginID}", Replacements.LoginID.ToString());
            sb.Replace("{Password}", star);
            sb.Replace("{PinPassword}", star);
            sb.Replace("{AccountNo}", Replacements.AccountNo);
            sb.Replace("{LiveID}", Replacements.LiveID);
            sb.Replace("{TID}", Convert.ToString(Replacements.TID));
            sb.Replace("{TransactionID}", Replacements.TransactionID);
            sb.Replace("{BankRequestStatus}", Replacements.RequestStatus);
            sb.Replace("{OutletID}", Replacements.OutletID);
            sb.Replace("{OutletMobile}", Replacements.OutletMobile);
            sb.Replace("{RejectReason}", Replacements.KycRejectReason);
            sb.Replace("{DATETIME}", Replacements.DATETIME);
            sb.Replace("{Duration}", Replacements.Duration);
            return Convert.ToString(sb);
        }

        #region NotifyModel Data
        public async Task<IResponse<IEnumerable<NotifyModel>>> GetNotify()
        {
            var res = new Response<IEnumerable<NotifyModel>>();
            try
            {
                var dbparams = new DynamicParameters();
                res.Result = await _dapper.GetAllAsync<NotifyModel>("proc_SelectNotify", dbparams, CommandType.StoredProcedure);
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
                    WID = item.WID,
                   
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


                string MailFooter = " <tr><td bgcolor='#f4f4f4' align='center' style='padding: 30px 10px 20px 10px;'><table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'><tbody><tr><td bgcolor = '#FFECD1' align = 'center' style = 'padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;margin-bottom:20px;'><h2 style = 'font-size: 20px; font-weight: 400; color: #111111; margin: 0;'> Need more help?</h2><p style='margin:0;'><a href = 'https://lapurobotics.com/' target='_blank' style = 'color: #2262c6;'>{Domain}</a></p></td></tr></tbody></table></td></tr> ";
                MailFooter = MailFooter.Replace("{Domain}", "https://lapurobotics.com/");

                var emailresponse = SendEMailAsync(mailSetting, item.SendTo, null, item.Subject, item.Message, item.WID, "LogoURL", true, MailFooter).Result;

                SendEmail sendEmail = new SendEmail
                {
                    From = mailSetting.FromEmail,
                    Body = item.Message,
                    Recipients = item.SendTo,
                    Subject = item.Subject,
                    IsSent = true,
                    WID = item.WID,
                    NotifyID = item.ID,
                    Response = emailresponse?"Success":"Failed"
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
                        SMS = item.Message
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
                dbparams.Add("WID", Response.WID, DbType.Int32);
                dbparams.Add("Status", Response.Status, DbType.String);
                dbparams.Add("TransactionID", Response.TransactionID, DbType.String);
                dbparams.Add("Response", Response.Response, DbType.String);
                dbparams.Add("ResponseID", Response.ResponseID, DbType.String);
                dbparams.Add("ReqURL", Response.ReqURL, DbType.String);
                dbparams.Add("SocialAlertType", Response.SocialAlertType, DbType.String);
           
               // var res = _dapper.Insert<CommonResponse>("proc_SaveSMSResponse", dbparams,  CommandType.StoredProcedure);
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
                dbparams.Add("WID", sendEmail.WID, DbType.Int32);
                dbparams.Add("NotifyID", sendEmail.NotifyID, DbType.Int32);
                dbparams.Add("Response", sendEmail.Response, DbType.String);
             //   var res =_dapper.Insert<CommonResponse>("proc_SaveEmailesponse", dbparams, CommandType.StoredProcedure);
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
                Tp_ReplaceKeywords.Rows.Add(AddKeyAndValue(MessageTemplateKeywords.Password, Password));
                Tp_ReplaceKeywords.Rows.Add(AddKeyAndValue(MessageTemplateKeywords.PinPassword, string.Empty));
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
                WID = WID
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
                    ApiResp =await AppWebRequest.O.CallUsingHttpWebRequest_GET(_req.SmsURL);
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
            bool IsSent = false;
            if (setting.FromEmail != null && setting.Port != 0 && setting.Password != null && setting.HostName != null)
            {
                if (IsHTML)
                {
                    Body = "HTML Template".Replace("{BODY}", Body).Replace("{Footer}", MailFooter).ToString();
                }
                try
                {
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(setting.FromEmail, "Lapu-Robotics"),
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
                        Credentials = new NetworkCredential(string.IsNullOrEmpty(setting.MailUserID) ? setting.FromEmail : setting.MailUserID, setting.Password)
                    };
                    if (setting.IsSSL)
                    {
                        smtpClient.EnableSsl = setting.IsSSL;
                    }
                    try
                    {
                        Task.Factory.StartNew(() => smtpClient.Send(mailMessage));
                        IsSent = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {

                }
            }
            return IsSent;
        }

    }
}
