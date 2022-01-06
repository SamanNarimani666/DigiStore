using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Enums.Payment;
using Microsoft.AspNetCore.Http;
using ZarinpalSandbox;

namespace DigiStore.Application.Services.Implementations
{
    public class PaymentSerivce : IPaymentSerivce
    {
        public PaymentStatus CreatePaymentRequest(string merchantId, int amount, string description, string callbackUrl,
            ref string redirectUrl, string userEmail = null, string userMobile = null)
        {
            var payment = new Payment(amount);
            var res = payment.PaymentRequest(description, callbackUrl, userEmail, userMobile);
            if (res.Result.Status == (int)PaymentStatus.St100)
            {
                redirectUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                return (PaymentStatus)res.Result.Status;
            }
            return (PaymentStatus)res.Result.Status;
        }

        public PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long redId)
        {
            var payment = new Payment(amount);
            var res = payment.Verification(authority).Result;
            redId = res.RefId;
            return (PaymentStatus)res.Status;

        }

        public string GetAuthorityCodeFromCallback(HttpContext context)
        {
            if (context.Request.Query["Status"] != "" &&
                    context.Request.Query["Status"].ToString().ToLower() == "ok"
                    && context.Request.Query["Authority"] != "")
            {
                string authority = context.Request.Query["Authority"];
                return authority.Length == 36 ? authority : null;
            }
            return null;
        }
    }
}
