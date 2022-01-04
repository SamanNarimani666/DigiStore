using System;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Application.Utils;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Store;
using DigiStore.Domain.IRepositories.SalesOrder;
using DigiStore.Domain.IRepositories.SalesOrderDetail;
using DigiStore.Domain.IRepositories.SellerWallet;
using DigiStore.Domain.ViewModels.Order;

namespace DigiStore.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        #region Constructor
        private readonly ISalesOrderHeaderRepository _orderHeaderRepository;
        private readonly ISalesOrderDetailRepository _orderDetailRepository;
        private readonly ISellerWalletRepository _sellerWalletRepository;
        public OrderService(ISalesOrderHeaderRepository orderHeaderRepository, ISalesOrderDetailRepository orderDetailRepository, ISellerWalletRepository sellerWalletRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailRepository = orderDetailRepository;
            _sellerWalletRepository = sellerWalletRepository;
        }
        #endregion

        #region OrderHeader
        #region AddOrderForUser
        public async Task<int> AddOrderForUser(int userId)
        {
            var order = new SalesOrderHeader { UserId = userId };
            await _orderHeaderRepository.AddSalesOrderHeader(order);
            await _orderHeaderRepository.Save();
            return order.SalesOrderId;
        }
        #endregion

        #region GetUserLastOpenOrder
        public async Task<SalesOrderHeader> GetUserLastOpenOrder(int userId)
        {
            if (!await _orderHeaderRepository.IsOpenSalesOrderHeaderByUser(userId))
                await AddOrderForUser(userId);

            var userOpenOrder = await _orderHeaderRepository.GetSalesOrderHeaderByUserId(userId);

            return userOpenOrder;
        }
        #endregion
        #endregion

        #region OrderDetail

        #region AddProductToOpenOrder
        public async Task AddProductToOpenOrder(int userId, AddProductToOrderViewModel addProductToOrder)
        {
            var opnOprder = await GetUserLastOpenOrder(userId);
            var similarOrder = opnOprder.SalesOrderDetails.SingleOrDefault(p =>
                p.ProductId == addProductToOrder.ProductId && p.ColorId == addProductToOrder.ProductColorId);
            try
            {
                if (similarOrder == null)
                {
                    var orderDetial = new SalesOrderDetail()
                    {
                        SalesOrderId = opnOprder.SalesOrderId,
                        ProductId = addProductToOrder.ProductId,
                        ColorId = addProductToOrder.ProductColorId,
                        OrderQty = addProductToOrder.OrderQty,

                    };
                    await _orderDetailRepository.AddOrderDetail(orderDetial);

                }
                else
                {
                    similarOrder.OrderQty += addProductToOrder.OrderQty;
                    similarOrder.ModifiedDate = DateTime.Now;
                    _orderDetailRepository.UpdateSalesOrderDetail(similarOrder);
                }
                await _orderHeaderRepository.Save();

            }
            catch
            {

            }
        }
        #endregion

        #region GetUserOpenOrderDetials
        public async Task<UserOpenOrderViewModel> GetUserOpenOrderDetials(int userId)
        {
            var oprnOrder = await GetUserLastOpenOrder(userId);
            if (oprnOrder == null) return null;
            return new UserOpenOrderViewModel()
            {
                Description = oprnOrder.Descriptions,
                UserId = userId,
                Details = oprnOrder.SalesOrderDetails.Select(s =>
                {
                    return new UserOpenOrderDetailItemViewModel()
                    {
                        Qty = s.OrderQty,
                        DetialId = s.SalesOrderDetailId,
                        ColorCode = s.Color?.ColorCode,
                        ProductColorId = s.ColorId,
                        ProductColorPrice = s.Color?.Price ?? 0,
                        ProductId = s.ProductId,
                        ProductPrice = s.Product.Price,
                        ProductTitle = s.Product.Name,
                        ProductImageName = s.Product.ImageName
                    };
                }).ToList()
            };
        }
        #endregion

        #region RemoveOrderDetial
        public async Task<bool> RemoveOrderDetial(int detialId, int userId)
        {
            var openOrder = await GetUserLastOpenOrder(userId);
            var orderDetial = openOrder.SalesOrderDetails.SingleOrDefault(p => p.SalesOrderDetailId == detialId);
            if (orderDetial == null) return false;
            try
            {

                _orderDetailRepository.DeleteSalesOrderDetail(orderDetial);
                await _orderDetailRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ChangeOrderQty
        public async Task ChangeOrderQty(int detialId, int userId, int qty)
        {
            var openOrder = await GetUserLastOpenOrder(userId);
            var detial = openOrder.SalesOrderDetails.SingleOrDefault(p => p.SalesOrderDetailId == detialId);
            if (detial != null)
            {
                if (qty > 0)
                {
                    detial.OrderQty = qty;
                    detial.ModifiedDate = DateTime.Now;
                }
                else
                {
                    _orderDetailRepository.DeleteSalesOrderDetail(detial);
                }
                await _orderDetailRepository.Save();
            }


        }
        #endregion

        #region GetTotalOrderPriceForPayment
        public async Task<int> GetTotalOrderPriceForPayment(int userId)
        {
            var openOrder = await GetUserLastOpenOrder(userId);
            int totalPrice = 0;
            foreach (var detial in openOrder.SalesOrderDetails)
            {
                var oneProductPrice = detial.Color != null ? detial.Product.Price + detial.Color.Price : detial.Product.Price;
                totalPrice += oneProductPrice * detial.OrderQty;
            }
            return totalPrice;
        }
        #endregion

        #region PayOrderProductPriceToSeller
        public async Task PayOrderProductPriceToSeller(int userId)
        {
            var openOrder = await GetUserLastOpenOrder(userId);
            foreach (var detials in openOrder.SalesOrderDetails)
            {
                var productPrice = detials.Product.Price;
                var productColorPrice = detials.Color?.Price ?? 0;
                var discunt = 0;
                var totlaPrice = detials.OrderQty * ((productPrice + productColorPrice) - discunt);
                await _sellerWalletRepository.AddSellerWallet(new SellerWallet()
                {
                    SellerId = detials.Product.SellerId,
                    Price = (int)Math.Ceiling(totlaPrice + detials.Product?.SiteProfile ?? 1 / (double)100),
                    TransactionType = (byte)TransactionType.Deposit,
                    Descriptions = $"فروش محصول {detials.Product.Name}  با مبلغ {totlaPrice}  در تاریخ{DateTime.Now.ToStringShamsiDate()} به تعداد  {detials.OrderQty}  عدد با سهام در نظر گرفته شده {(100) - detials.Product.SiteProfile}در صد "
                });
                detials.Price = totlaPrice;
                detials.ColorPrice = productColorPrice;
                detials.ModifiedDate = DateTime.Now;
                _orderDetailRepository.UpdateSalesOrderDetail(detials);

            }
            await _orderDetailRepository.Save();
            openOrder.IsPaiy = true;
            openOrder.ModifiedDate = DateTime.Now;
            _orderHeaderRepository.UpdateSalesOrderHeader(openOrder);
            await _orderHeaderRepository.Save();

        }

        #endregion

        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _orderHeaderRepository.DisposeAsync();
            await _orderDetailRepository.DisposeAsync();
        }
        #endregion
    }
}
