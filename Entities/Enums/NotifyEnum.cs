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
        FundTransfer = 2,
        FundReceive = 3,
        OTP = 4,
        FundDebit = 5,
        FundCredit = 6,
        RechargeAccept = 7,
        RechargeSuccess = 8,
        RechargeFailed = 9,
        OperatorUPMessage = 10,
        OperatorDownMessage = 11,
        RechargeRefund = 12,
        InvoiceFundCredit = 13,
        LowBalanceFormat = 14,
        ForgetPass = 15,
        senderRegistrationOTP = 16,
        BenificieryRegistrationOTP = 17,
        KYCApproved = 18,
        KYCReject = 19,
        FundOrderAlert = 20,
        UserPartialApproval = 21,
        UserSubscription = 22,
        ThankYou = 23,
        CallNotPicked = 24,
        BirthdayWish = 25,
        MarginRevised = 26,
        RechargeRefundReject = 27,
        Payout = 28,
        BBPSSuccess = 29,
        BBPSComplainRegistration = 30,
        PendingRechargeNotification = 31,
        PendingRefundNotification = 32,
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
