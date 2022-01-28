using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Contacts;
using DigiStore.Domain.ViewModels.SiteSetting;
using DigiStore.Domain.ViewModels.Slider;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Interfaces
{
    public interface ISiteService:IAsyncDisposable
    {
        Task<CreateContactResult> CreateContact(CreateContactUsViewModel createContactUs,string userIp);
        Task<SiteSetting> GetDefaultSiteSetting();
        Task<List<Slider>> GetAllActiveSlider();
        Task<GetSiteInformation> GetDefaultSiteInformation();
        Task<EditSiteSettingResult> EditSiteSetting(GetSiteInformation editSiteInformation);
        Task<FilterContactUsViewModel> FilterContactUs(FilterContactUsViewModel filterContactUs);
        Task<ContactusForAnsweViewModel> GetContactusForAnswe(int contactusId);
        Task<AnswerContactusResult> AnswerContactUs(ContactusForAnsweViewModel Answer);
        Task<FilterSliderViewModel> FilterSlider(FilterSliderViewModel filterSlider);
        Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider,IFormFile sliderImage);
        Task<bool> DeleteSlider(int sliderId);
        Task<bool> RestoreSlider(int sliderId);
        Task<EditSliderViewModel> SliderInfoForEdit(int sliderId);
        Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider, IFormFile sliderImage);
    }
}
