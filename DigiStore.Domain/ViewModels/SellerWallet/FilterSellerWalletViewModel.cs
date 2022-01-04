using System.Collections.Generic;
using DigiStore.Domain.Enums.Store;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.SellerWallet
{
    public class FilterSellerWalletViewModel : BasePaging
    {
        public int? SellerId { get; set; }

        public int? PriceFrom { get; set; }
        public int? PriceTo{ get; set; }
        public TransactionType TransactionType { get; set; }

        public List<Entities.SellerWallet> SellerWallets { get; set; }
        #region methods

        public FilterSellerWalletViewModel SetSellers(List<Entities.SellerWallet> sellerWallets)
        {
            this.SellerWallets = sellerWallets;
            return this;
        }

        public FilterSellerWalletViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion


    }
}
