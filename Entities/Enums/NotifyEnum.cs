using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities.Enums
{
    public enum NotifyEnum
    {
       
    }
    public enum MessageFormat
    {
        Registration = 1,
        OTP = 2,
        FundDebit = 3,
        FundCredit = 4,
        ForgetPass = 5,
        ThankYou =6,
        OrderPlaced = 7
    }
    public enum CommunicationMode
    {
        SMS = 1,
        Email = 2,
        Social = 3,

    }
    public class SMSResponseTYPE
    {
        public static int SEND = 1;
        public static int UNSENT = 2;
        public static int DELIVERED = 3;
        public static int UNDELIVERED = 4;
        public static int FAILED = -1;
        public static int RESEND = 5;
    }
    public enum MessageTemplateType
    {
        SMS = 1,
        Email = 2,
        Alert = 3,
        WebNotification = 4,
        Social = 5,
        Hangout = 6,
        Whatsapp = 7,
        Telegram = 8
    }

}
