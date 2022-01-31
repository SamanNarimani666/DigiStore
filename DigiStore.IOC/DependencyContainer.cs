using DigiStore.Application.Security.PassWordHashing;
using DigiStore.Application.Senders;
using DigiStore.Application.Services.Implementations;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Data.Context;
using DigiStore.Data.Repositories.Address;
using DigiStore.Data.Repositories.AddressCity;
using DigiStore.Data.Repositories.AddressState;
using DigiStore.Data.Repositories.Brand;
using DigiStore.Data.Repositories.Category;
using DigiStore.Data.Repositories.ContactUs;
using DigiStore.Data.Repositories.FavoriteProductUser;
using DigiStore.Data.Repositories.Permission;
using DigiStore.Data.Repositories.Product;
using DigiStore.Data.Repositories.ProductColor;
using DigiStore.Data.Repositories.Productcomment;
using DigiStore.Data.Repositories.ProductDiscount;
using DigiStore.Data.Repositories.ProductDiscountUse;
using DigiStore.Data.Repositories.ProductFeature;
using DigiStore.Data.Repositories.ProductGallery;
using DigiStore.Data.Repositories.ProductRating;
using DigiStore.Data.Repositories.ProductVisited;
using DigiStore.Data.Repositories.Role;
using DigiStore.Data.Repositories.RolePermission;
using DigiStore.Data.Repositories.SalesInforamtion;
using DigiStore.Data.Repositories.SalesOrder;
using DigiStore.Data.Repositories.SalesOrderDetail;
using DigiStore.Data.Repositories.SelectedProductCategory;
using DigiStore.Data.Repositories.Seller;
using DigiStore.Data.Repositories.SellerWallet;
using DigiStore.Data.Repositories.SiteSetting;
using DigiStore.Data.Repositories.Slider;
using DigiStore.Data.Repositories.Ticket;
using DigiStore.Data.Repositories.User;
using DigiStore.Data.Repositories.UserRole;
using DigiStore.Domain.IRepositories.Address;
using DigiStore.Domain.IRepositories.AddressCity;
using DigiStore.Domain.IRepositories.AddressState;
using DigiStore.Domain.IRepositories.Brand;
using DigiStore.Domain.IRepositories.Category;
using DigiStore.Domain.IRepositories.ContactUs;
using DigiStore.Domain.IRepositories.FavoriteProductUser;
using DigiStore.Domain.IRepositories.Permission;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.ProductColor;
using DigiStore.Domain.IRepositories.Productcomment;
using DigiStore.Domain.IRepositories.ProductDiscount;
using DigiStore.Domain.IRepositories.ProductDiscountUse;
using DigiStore.Domain.IRepositories.ProductFeature;
using DigiStore.Domain.IRepositories.ProductGallery;
using DigiStore.Domain.IRepositories.ProductRating;
using DigiStore.Domain.IRepositories.ProductVisited;
using DigiStore.Domain.IRepositories.Role;
using DigiStore.Domain.IRepositories.RolePermission;
using DigiStore.Domain.IRepositories.SalesInforamtion;
using DigiStore.Domain.IRepositories.SalesOrder;
using DigiStore.Domain.IRepositories.SalesOrderDetail;
using DigiStore.Domain.IRepositories.SelectedProductCategory;
using DigiStore.Domain.IRepositories.Seller;
using DigiStore.Domain.IRepositories.SellerWallet;
using DigiStore.Domain.IRepositories.SiteSetting;
using DigiStore.Domain.IRepositories.Ticket;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.IRepositories.UserRole;
using DigiStore.Domain.ViewModels.Slider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigiStore.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services,string connectionString)
        {
            #region Context
            //DB Context Options
            services.AddDbContext<DigiStore_DBContext>(option => { option.UseSqlServer(connectionString); });
            #endregion

            #region Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IProductCategoryRepository,ProductCategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IStateRepository,StateRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ISelectedProductCategoryRepository,SelectedProductCategoryRepository>();
            services.AddScoped<IProductColorRepository, ProductColorRepository>();
            services.AddScoped<IProductGalleryRepository, ProductGalleryRepository>();
            services.AddScoped<IProductVisitedRepository, ProductVisitedRepository>();
            services.AddScoped<IProductFeatureRepository, ProductFeatureRepository>();
            services.AddScoped<ISalesOrderHeaderRepository, SalesOrderHeaderRepository>();
            services.AddScoped<ISalesOrderDetailRepository, SalesOrderDetailRepository>();
            services.AddScoped<ISellerWalletRepository, SellerWalletRepository>();
            services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
            services.AddScoped<IProductDiscountUseRepository, ProductDiscountUseRepository>();
            services.AddScoped<IPaymentSerivce,PaymentSerivce>();
            services.AddScoped<IFavoriteProductUserRepository, FavoriteProductUserRepository>();
            services.AddScoped<IProductcommentRepository, ProductcommentRepository>();
            services.AddScoped<ISalesInforamtionRepository, SalesInforamtionRepository>();
            services.AddScoped<IRoleRepository,RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
            services.AddScoped<IProductRatingRepository, ProductRatingRepository>();
            services.AddScoped<ISiteSliderRepository, SiteSliderRepository>();
            #endregion

            #region Tools
            services.AddSingleton<IPasswordHelper, PasswordHelper>();
            services.AddSingleton<ISender, EmailSender>();
            #endregion

            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService,TicketService>();
            services.AddScoped<IAddressService,AddressService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBranadService, BranadService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISellerWalletService, SellerWalletService>();
            services.AddScoped<IProductDiscountService, ProductDiscountService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IReportService, ReportService>();
            #endregion
        }
    }
}
