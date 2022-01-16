using System;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Application.Utils;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Store;
using DigiStore.Domain.IRepositories.Address;
using DigiStore.Domain.IRepositories.ProductDiscount;
using DigiStore.Domain.IRepositories.ProductDiscountUse;
using DigiStore.Domain.IRepositories.SalesInforamtion;
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
        private readonly IProductDiscountRepository _discountRepository;
        private readonly IProductDiscountUseRepository _discountUseRepository;
        private readonly ISalesInforamtionRepository _salesInforamtionRepository;
        private readonly IAddressRepository _addressRepository;
        public OrderService(ISalesOrderHeaderRepository orderHeaderRepository, ISalesOrderDetailRepository orderDetailRepository, ISellerWalletRepository sellerWalletRepository, IProductDiscountRepository discountRepository, IProductDiscountUseRepository discountUseRepository, ISalesInforamtionRepository salesInforamtionRepository, IAddressRepository addressRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailRepository = orderDetailRepository;
            _sellerWalletRepository = sellerWalletRepository;
            _discountRepository = discountRepository;
            _discountUseRepository = discountUseRepository;
            _salesInforamtionRepository = salesInforamtionRepository;
            _addressRepository = addressRepository;
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
                OrderId = oprnOrder.SalesOrderId,
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
                        ProductImageName = s.Product.ImageName,
                        DiscountPercentage = s.Product.ProductDiscounts.OrderByDescending(p => p.ModifiedDate).FirstOrDefault(p => p.ExpierDate > DateTime.Now && p.DiscountNumber - p.ProductDiscountUses.Count >= 0)?.Percentage
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
            var discount = 0;

            foreach (var detials in openOrder.SalesOrderDetails)
            {
                var oneProductPrice = (detials.Color != null ? detials.Product.Price + detials.Color.Price : detials.Product.Price);
                var productDiscount =
                    await _discountRepository.GetProductDiscountByProductId(detials.Product.ProductId);
                if (productDiscount != null)
                {
                    discount = (int)Math.Ceiling(oneProductPrice * productDiscount.Percentage / ((decimal)100));
                }

                totalPrice += detials.OrderQty * (oneProductPrice - discount);
                discount = 0;
            }
            openOrder.OrderSum = totalPrice;
            openOrder.ModifiedDate = DateTime.Now;
            _orderHeaderRepository.UpdateSalesOrderHeader(openOrder);
            await _orderHeaderRepository.Save();
            return totalPrice;
        }
        #endregion

        #region PayOrderProductPriceToSeller
        public async Task PayOrderProductPriceToSeller(int userId, long trackingCode)
        {
            var openOrder = await GetUserLastOpenOrder(userId);
            foreach (var detials in openOrder.SalesOrderDetails)
            {
                var productPrice = detials.Product.Price;
                var productColorPrice = detials.Color?.Price ?? 0;
                var discunt = 0;
                var totalPrice = detials.OrderQty * ((productPrice + productColorPrice) - discunt);
                var productDiscount =
                    await _discountRepository.GetProductDiscountByProductId(detials.Product.ProductId);
                if (productDiscount != null)
                {
                    discunt = (int)Math.Ceiling(totalPrice * productDiscount.Percentage / ((decimal)100));
                    try
                    {
                        var newDiscountUse = new ProductDiscountUse()
                        {
                            UserId = userId,
                            ProductDiscountId = productDiscount.ProductDiscountId,
                            CreateDate = DateTime.Now,
                        };
                        await _discountUseRepository.AddProductDiscountUse(newDiscountUse);
                        await _discountUseRepository.Save();
                    }
                    catch { }
                }

                var totalPriceWithDiscount = totalPrice - discunt;
                await _sellerWalletRepository.AddSellerWallet(new SellerWallet()
                {
                    SellerId = detials.Product.SellerId,
                    Price = (int)Math.Ceiling(totalPriceWithDiscount * (100 - detials.Product?.SiteProfile ?? 0) / (double)100),
                    TransactionType = (byte)TransactionType.Deposit,
                    Descriptions = $"فروش محصول {detials.Product.Name}  با مبلغ {totalPriceWithDiscount}  در تاریخ{DateTime.Now.ToStringShamsiDate()} به تعداد  {detials.OrderQty}  عدد با سهام در نظر گرفته شده {(100) - detials.Product.SiteProfile}در صد "
                });
                detials.Price = totalPriceWithDiscount;
                detials.ColorPrice = productColorPrice;
                detials.ModifiedDate = DateTime.Now;
                _orderDetailRepository.UpdateSalesOrderDetail(detials);

            }
            await _orderDetailRepository.Save();
            openOrder.IsPaiy = true;
            openOrder.TracingCode = Convert.ToString(trackingCode);
            openOrder.ModifiedDate = DateTime.Now;
            _orderHeaderRepository.UpdateSalesOrderHeader(openOrder);
            await _orderHeaderRepository.Save();

        }
        #endregion

        #endregion

        #region GetOpenOrderUserForAddInformation
        public async Task<CreateOrderInforamtionViewModel> GetOpenOrderUserForAddInformation(int orderId, int userId)
        {
            var OpenOrders = await GetUserOpenOrderDetials(userId);
            if (OpenOrders == null) return null;
            if (OpenOrders.UserId != userId) return null;
            if (OpenOrders.OrderId != orderId) return null;
            return new CreateOrderInforamtionViewModel()
            {
                OrderId = OpenOrders.OrderId,
                TotlaPrice = OpenOrders.GetTotalPrice() - OpenOrders.GetTotalDiscounts()
            };
        }
        #endregion
        #region CreateOrderInforamtion
        public async Task<CreateOrderInforamtionResult> CreateOrderInforamtion(CreateOrderInforamtionViewModel createOrderInforamtion, int userId)
        {
            var openOrder = await GetUserLastOpenOrder(userId);
            if (openOrder == null) return CreateOrderInforamtionResult.NotFoundUserOpenOrder;
            var address = await _addressRepository.GetAddressById(createOrderInforamtion.AddressId);
            if (address == null) return CreateOrderInforamtionResult.NotFoundAddress;
            if (address.UserId != userId) return CreateOrderInforamtionResult.NotFoundAddress;
            try
            {
                var newInfoOrder = new SalesInforamtion()
                {
                    AddressId = createOrderInforamtion.AddressId,
                    UserId = userId,
                    SalesOrderId = createOrderInforamtion.OrderId,
                    ReceiverMobile = createOrderInforamtion.ReceiverMobile,
                    ReceiverName = createOrderInforamtion.ReceiverName,
                    ReceiverNaationalId = createOrderInforamtion.ReceiverNaationalId
                };
                    await _salesInforamtionRepository.AddSalesInforamtion(newInfoOrder);
                    await _salesInforamtionRepository.Save();
                return CreateOrderInforamtionResult.Success;
            }
            catch
            {
                return CreateOrderInforamtionResult.Error;
            }

        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _orderHeaderRepository.DisposeAsync();
            await _orderDetailRepository.DisposeAsync();
            await _sellerWalletRepository.DisposeAsync();
            await _discountRepository.DisposeAsync();
            await _discountUseRepository.DisposeAsync();
            await _salesInforamtionRepository.DisposeAsync();
            await _addressRepository.DisposeAsync();
        }
        #endregion
    }
}
