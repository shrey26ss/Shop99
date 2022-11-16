using AutoMapper;
using Entities.Models;
using Microsoft.Win32;
using Service.EmailConfig;
using Services.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Identity
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Service.EmailConfig.EmailConfig, EmailSettings>();
            CreateMap<ApplicationUser, RegisterViewModel>();
            CreateMap<ApplicationUser, UserUpdateRequest>().ReverseMap();
            CreateMap<ApplicationUser, Register>();
            CreateMap<ApplicationUserProcModel, ApplicationUser>().ReverseMap();
            //CreateMap<WebScrapModel.VerifyOTPRequest, WebScrapModel.RefreshTokenRequest>().ReverseMap();
            //CreateMap<WebScrapModel.RefreshTokenRequest, WebScrapModel.GroupInfoRequest>().ReverseMap();
            //CreateMap<WebScrapModel.RefreshTokenRequest, WebScrapModel.UpdateSessionRequest>().ReverseMap();
            //CreateMap<WebScrapModel.VerifyOTPRequest, WebScrapModel.MerchantProfileRequest>().ReverseMap();
            //CreateMap<WebScrapModel.MerchantProfileRequest, WebScrapModel.StoreListRequest>().ReverseMap();
            //CreateMap<WebScrapModel.StoreListRequest, WebScrapModel.QRPosListRequest>().ReverseMap();
            //CreateMap<InitiateTransactionRequest, TransactionRequest>();
            //CreateMap<PaymentGatewayModel, StatusCheckRequest>().ReverseMap();
            //CreateMap<UPISettingWithTIDDetail, WebScrapModel.RefreshTokenRequest>().ReverseMap();
            //CreateMap<UPISetting, WebScrapModel.RefreshTokenRequest>().ReverseMap();
        }
    }
}
