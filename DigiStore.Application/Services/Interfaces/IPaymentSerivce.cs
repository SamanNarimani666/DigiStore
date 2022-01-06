using System;
using DigiStore.Domain.Enums.Payment;
using Microsoft.AspNetCore.Http;
using ZarinpalSandbox.Models;


namespace DigiStore.Application.Services.Interfaces
{
    public interface IPaymentSerivce
    {
        PaymentStatus CreatePaymentRequest(string merchantId,int amount,string description,string callbackUrl,ref string redirectUrl,string userEmail=null,string userMobile=null);
        PaymentStatus PaymentVerification(string merchantId,string authority,int amount,ref long redId);
        string GetAuthorityCodeFromCallback(HttpContext context);
    }
}
