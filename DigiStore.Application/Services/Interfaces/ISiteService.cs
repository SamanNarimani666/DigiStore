using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Contacts;

namespace DigiStore.Application.Services.Interfaces
{
    public interface ISiteService:IAsyncDisposable
    {
        Task<CreateContactResult> CreateContact(CreateContactUsViewModel createContactUs,string userIp);
        Task<SiteSetting> GetDefaultSiteSetting();
        Task<List<Slider>> GetAllActiveSlider();


    }
}
