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
using DigiStore.Data.Repositories.Guarantee;
using DigiStore.Data.Repositories.Product;
using DigiStore.Data.Repositories.ProductColor;
using DigiStore.Data.Repositories.SelectedProductCategory;
using DigiStore.Data.Repositories.Seller;
using DigiStore.Data.Repositories.Ticket;
using DigiStore.Data.Repositories.User;
using DigiStore.Domain.IRepositories.Address;
using DigiStore.Domain.IRepositories.AddressCity;
using DigiStore.Domain.IRepositories.AddressState;
using DigiStore.Domain.IRepositories.Brand;
using DigiStore.Domain.IRepositories.Category;
using DigiStore.Domain.IRepositories.Guarantee;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.ProductColor;
using DigiStore.Domain.IRepositories.SelectedProductCategory;
using DigiStore.Domain.IRepositories.Seller;
using DigiStore.Domain.IRepositories.Ticket;
using DigiStore.Domain.IRepositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigiStore.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services,string connectionString)
        {
            //DB Context Options
            services.AddDbContext<DigiStore_DBContext>(option => { option.UseSqlServer(connectionString); });

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
            services.AddScoped<IProductGuaranteeRepository,ProductGuaranteeRepository>();

            services.AddSingleton<IPasswordHelper, PasswordHelper>();
            services.AddSingleton<ISender, EmailSender>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService,TicketService>();
            services.AddScoped<IAddressService,AddressService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBranadService, BranadService>();

        }
    }
}
