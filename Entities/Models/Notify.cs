using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Entities.Models
{
    public class SMSEmailWhatsappNotification
    {
        public int UserID { get; set; }
        public MessageFormat FormatID { get; set; }
        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }
        public bool IsWhatsapp { get; set; }
    }
    public class AlertReplacementModel
    {
        public string KycRejectReason { get; set; }
        public string Message { get; set; }
        public string URL { get; set; }
        public int LoginID { get; set; }
        public string LoginUserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailID { get; set; }
        public decimal Amount { get; set; }
        public string OTP { get; set; }
        public int UserID { get; set; }
        public string TransactionID { get; set; }
        public string Company { get; set; }
        public string CompanyDomain { get; set; }
        public string CompanyAddress { get; set; }
        public string BrandName { get; set; }
        public string SupportNumber { get; set; }
        public string SupportEmail { get; set; }
        public string DATETIME { get; set; }
        public string RequestIP { get; set; }
        public string LiveID { get; set; }
        public MessageFormat FormatID { get; set; }
        public string NotificationTitle { get; set; }
        public string MobileNos { get; set; }
        public string SocialIDs { get; set; }
        public string WhatsappNo { get; set; }
        public string WhatsappConversationID { get; set; }
        public string TelegramNo { get; set; }
        public string HangoutNo { get; set; }
        public int SocialAlertType { get; set; }
        public string SocialID { get; set; }
        public string Subject { get; set; }
        public List<string> bccList { get; set; }
    }


    public class NotifyModel
    {
        public int ID { get; set; }     
        public int UserID { get; set; }
        public int APIID { get; set; }
        public int CommunicationMode { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int ApiType { get; set; }
        public string EmailConfiguration { get; set; }
        public string ApiUrl { get; set; }
        public string Method { get; set; }
        public string EncryptedData { get; set; }
        public string SendTo { get; set; }
        public string SendFrom { get; set; }
        public string BCC { get; set; }
        public string ButtonJson { get; set; }
        public string WhatsappTitle { get; set; }
        public string WhatsappTitleType { get; set; }
        public string WhatsappFooter { get; set; }
        public string ImageUrl { get; set; }
    }


    public class MessageTemplate
    {
        public MessageTemplateType TemplateType { get; set; }
        
        public int FormatID { get; set; }
        public bool IsSMSEnable { get; set; }
        public string SMSTemplateID { get; set; }
        public string SMSTemplate { get; set; }
        public bool IsEmailEnable { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTemplate { get; set; }
        public bool IsEnableAppNotification { get; set; }
        public bool IsEnableWebNotification { get; set; }
        public string WebNotificationTemplate { get; set; }
        public bool IsSocialAlertEnable { get; set; }
        public bool IsHangoutEnable { get; set; }
        public string HangoutTemplate { get; set; }
        public bool IsWhatsappEnable { get; set; }
        public string WhatsAppTemplateID { get; set; }
        public string WhatsappTemplate { get; set; }
        public bool IsTelegramEnable { get; set; }
        public string TelegramTemplate { get; set; }

        public string MasterTemplate { get; set; }
        public string ButtonJson { get; set; }
        public string WhatsappTitle { get; set; }
        public string WhatsappTitleType { get; set; }
        public string WhatsappFooter { get; set; }
    }
    public class SmsApi
    {
        public string StatusCode { get; set; }
        public int id { get; set; }
        public int apitype { get; set; }
        public string transactiontype { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string isactive { get; set; }
        public string isdefault { get; set; }
        public string isdeleted { get; set; }
       
        public string entryby { get; set; }
        public string entrydate { get; set; }
        public string modifyby { get; set; }
        public string modifydate { get; set; }
        public string apimethod { get; set; }
        public string restype { get; set; }
        public bool ismultipleallowed { get; set; }
        public string ApiCode { get; set; }

    }
    public class SMSSetting
    {
        public int ID { get; set; }
        public int Statuscode { get; set; }
        public string Msg { get; set; }
        public int FormatID { get; set; }
        public string Template { get; set; }
        public string Subject { get; set; }
        public bool IsEnableSMS { get; set; }
        public int APIType { get; set; }
        public string URL { get; set; }
        public string APIMethod { get; set; }
        public int APIID { get; set; }
        public int ResType { get; set; }
        public int SMSID { get; set; }
        public string SenderID { get; set; }
        public string MobileNos { get; set; }
        public string SMS { get; set; }
        public bool IsLapu { get; set; }
    }
    public class EmailApi
    {
        public int ID { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string SmtpUserName { get; set; }
        public int Port { get; set; }
 
        public string EntryByLT { get; set; }
        public int EntryBy { get; set; }
        public string EntryDate { get; set; }
        public string ModifyByLT { get; set; }
        public int ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsSSL { get; set; }
        public string MailUserID { get; set; }
        public string IsDefault { get; set; }
    }

    public class SocialAlertFormat
    {
        public bool IsSocialAlert { get; set; }
        public string SocialAlertTemplate { get; set; }
        public string ScanNo { get; set; }
        public string CountryCode { get; set; }
        public int SocialAlertAPIID { get; set; }
        public string ButtonJson { get; set; }
        public string WhatsappTitle { get; set; }
        public string WhatsappFooter { get; set; }
        public string WhatsappTitleType { get; set; }


    }
    public class SendSMSRequest
    {
        public int SMSID { get; set; }
        public int CommunicationMode { get; set; }
        public int APIID { get; set; }
        public string SMS { get; set; }
        public string SmsURL { get; set; }
        public string APIMethod { get; set; }
        public string TransactionID { get; set; }
        public string MobileNo { get; set; }
        public bool IsLapu { get; set; }
        public bool IsSend { get; set; }
        public string ApiResp { get; set; }
    }

    public class SMSResponse
    {
        public int SMSID { get; set; }
        public string MobileNo { get; set; }
        public string SMS { get; set; }
        public int Status { get; set; }
        public string TransactionID { get; set; }
        public string Response { get; set; }
        public string ResponseID { get; set; }
        public string ReqURL { get; set; }
        public int SocialAlertType { get; set; }
        public int NotifyID { get; set; }
    }
    public class SendEmail
    {
        public int ID { get; set; }
        public string From { get; set; }
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
    
        public string UserName { get; set; }
        public string UserMobileNo { get; set; }
        public string EmailID { get; set; }
        public string Message { get; set; }
        public int RequestMode { get; set; }
        public string RequestIP { get; set; }
        public string RequestPage { get; set; }
        public int NotifyID { get; set; }
        public string Response { get; set; }

    }
    public class ButtonAertHub
    {
        public string btn_type { get; set; }
        public string display_txt { get; set; }
        public string call { get; set; }
        public string url { get; set; }
    }
    public class WhatsappAPIAlertHubButtons
    {
        public string apiusername { get; set; }
        public string apipassword { get; set; }
        public string requestid { get; set; }
        public string jid { get; set; }
        public string messagetype { get; set; }
        public string content { get; set; }
        public string from { get; set; }
        public string titletype { get; set; }
        public string title { get; set; }
        public string footer { get; set; }
        public dynamic buttons { get; set; }
    }
    public class EmailSettingswithFormat
    {
        public int ID { get; set; }
        public int Statuscode { get; set; }
        public string Msg { get; set; }
        public int FormatID { get; set; }
        public string EmailTemplate { get; set; }
        public string Subject { get; set; }
        public bool IsEnableEmail { get; set; }
        public string FromEmail { get; set; }
        public string SaleEmail { get; set; }
        public string HostName { get; set; }
        public string SmtpUserName { get; set; }
        public int Port { get; set; }
        public bool IsSSL { get; set; }
        public string MailUserID { get; set; }
        public string Password { get; set; }

    }
    public class MessageTemplateKeywords
    {
        public const string Mobile = "{Mobile}";
        public const string Amount = "{Amount}";
        public const string ToMobileNo = "{ToMobileNo}";
        public const string TransactionID = "{TransactionID}";
        public const string Company = "{Company}";
        public const string CompanyDomain = "{CompanyDomain}";
        public const string OTP = "{OTP}";
        public const string UserName = "{UserName}";
        public const string CompanyMobile = "{CompanyMobile}";
        public const string CompanyEmail = "{CompanyEmail}";
        public const string LoginID = "{LoginID}";
        public const string Message = "{Message}";
        public const string UserEmail = "{UserEmail}";
        public const string SenderName = "{SenderName}";
        public const string TransMode = "{TransMode}";
    }
    public class SMSSendREQ
    {
        public int FormatType { get; set; }
        public string MobileNo { get; set; }
        public DataTable Tp_ReplaceKeywords { get; set; }
        public string GeneralSMS { get; set; }
        
    }
    public class WhatsappSetting
    {
        public AlertHub AlertHub { get; set; }
    }
    public class AlertHub
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
