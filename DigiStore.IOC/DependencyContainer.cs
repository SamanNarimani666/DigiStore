using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Application.Security.PassWordHashing;
using DigiStore.Application.Senders;
using DigiStore.Application.Services.Implementations;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Context.Entities;
using DigiStore.Data.Repositories.User;
using DigiStore.Domain.IRepositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigiStore.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services,string connectionString)
        {
            services.AddDbContext<DigiStore_DBContext>(option => { option.UseSqlServer(connectionString); });
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IPasswordHelper, PasswordHelper>();
            services.AddSingleton<ISender, EmailSender>();

            services.AddScoped<IUserService, UserService>();


        }
    }
}
