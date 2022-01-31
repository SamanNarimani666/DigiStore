using System;

namespace DigiStore.Domain.ViewModels.Seller
{
    public class SellerInfoViewModel:RequestSellerViewModel
    {
        public DateTime RequstDate { get; set; }
        public int  Wallet { get; set; }
    }
}
