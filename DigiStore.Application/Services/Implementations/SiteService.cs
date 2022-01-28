using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DigiStore.Application.Convertors;
using DigiStore.Application.Extensions;
using DigiStore.Application.Security;
using DigiStore.Application.Senders;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.ContactUs;
using DigiStore.Domain.IRepositories.SiteSetting;
using DigiStore.Domain.ViewModels.Contacts;
using DigiStore.Domain.ViewModels.SiteSetting;
using DigiStore.Domain.ViewModels.Slider;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region Contrcutor
        private readonly IContactUsRepository _contactUsRepository;
        private readonly ISiteSettingRepository _settingRepository;
        private readonly ISiteSliderRepository _siteSliderRepository;
        private readonly ISender _sender;
        public SiteService(IContactUsRepository contactUsRepository, ISiteSettingRepository settingRepository, ISiteSliderRepository siteSliderRepository, ISender sender)
        {
            _contactUsRepository = contactUsRepository;
            _settingRepository = settingRepository;
            _siteSliderRepository = siteSliderRepository;
            _sender = sender;
        }
        #endregion

        #region CreateContact
        public async Task<CreateContactResult> CreateContact(CreateContactUsViewModel createContactUs, string userIp)
        {
            var newContactUs = new ContactU()
            {
                Email = FixedText.FixEmail(createContactUs.Email.SanitizeText()),
                FullName = createContactUs.FullName.SanitizeText(),
                UserIp = userIp,
                Subject = createContactUs.Subject.SanitizeText(),
                Text = createContactUs.Text.SanitizeText()
            };
            try
            {
                await _contactUsRepository.AddContactUs(newContactUs);
                await _contactUsRepository.Save();
                return CreateContactResult.Success;
            }
            catch
            {
                return CreateContactResult.Error;
            }
        }
        #endregion

        #region GetDefaultSiteSetting
        public async Task<SiteSetting> GetDefaultSiteSetting()
        {
            return await _settingRepository.GetDefaultSiteSetting();
        }
        #endregion

        #region GetAllActiveSlider
        public async Task<List<Slider>> GetAllActiveSlider()
        {
            return await _siteSliderRepository.GetAllActiveSlider();
        }
        #endregion

        #region GetDefaultSiteInformation
        public async Task<GetSiteInformation> GetDefaultSiteInformation()
        {
            var siteSetting = await _settingRepository.GetDefaultSiteSetting();
            if (siteSetting == null) return null;
            return new GetSiteInformation()
            {
                Email = siteSetting.Email,
                Phone = siteSetting.Phone,
                Address = siteSetting.Address,
                AboutUs = siteSetting.AboutUs,
                CopyRight = siteSetting.CopyRight,
                FooterText = siteSetting.FooterText,
                SiteSettingId = siteSetting.SiteSettingId
            };
        }
        #endregion

        #region EditSiteSetting
        public async Task<EditSiteSettingResult> EditSiteSetting(GetSiteInformation editSiteInformation)
        {
            var mainSiteSetting = await _settingRepository.GetDefaultSiteSettingById(editSiteInformation.SiteSettingId);
            if (mainSiteSetting == null) return EditSiteSettingResult.NotFound;
            mainSiteSetting.Phone = editSiteInformation.Phone.SanitizeText();
            mainSiteSetting.Email = FixedText.FixEmail(editSiteInformation.Email.SanitizeText());
            mainSiteSetting.AboutUs = editSiteInformation.AboutUs.SanitizeText();
            mainSiteSetting.CopyRight = editSiteInformation.CopyRight.SanitizeText();
            mainSiteSetting.Address = editSiteInformation.Address.SanitizeText();
            mainSiteSetting.FooterText = editSiteInformation.FooterText.SanitizeText();
            mainSiteSetting.Address = editSiteInformation.Address;
            try
            {
                _settingRepository.EditSiteSetting(mainSiteSetting);
                await _settingRepository.Save();
                return EditSiteSettingResult.Success;
            }
            catch
            {
                return EditSiteSettingResult.Error;
            }
        }
        #endregion

        #region FilterContactUs
        public async Task<FilterContactUsViewModel> FilterContactUs(FilterContactUsViewModel filterContactUs)
        {
            return await _contactUsRepository.FilterContactUs(filterContactUs);
        }
        #endregion

        #region GetContactusForAnswe
        public async Task<ContactusForAnsweViewModel> GetContactusForAnswe(int contactusId)
        {
            var contactUs = await _contactUsRepository.GetContactUById(contactusId);
            if (contactUs == null) return null;
            return new ContactusForAnsweViewModel()
            {
                Text = contactUs.Text,
                Email = contactUs.Email,
                FullName = contactUs.FullName,
                Subject = contactUs.Subject,
                UserIp = contactUs.UserIp,
                ContactUsId = contactUs.ContactUsid
            };
        }
        #endregion

        #region AnswerContactUs
        public async Task<AnswerContactusResult> AnswerContactUs(ContactusForAnsweViewModel Answer)
        {
            var contactUs = await _contactUsRepository.GetContactUById(Answer.ContactUsId);
            if (contactUs == null) return AnswerContactusResult.NotFound;
            if (string.IsNullOrEmpty(Answer.AnswerText)) return AnswerContactusResult.TextIsEmpty;
            try
            {
                _sender.SendEmail(contactUs.Email, $":جواب تماس {contactUs.Subject}", Answer.AnswerText);
                return AnswerContactusResult.Success;
            }
            catch
            {
                return AnswerContactusResult.Error;
            }
        }
        #endregion

        #region FilterSlider
        public async Task<FilterSliderViewModel> FilterSlider(FilterSliderViewModel filterSlider)
        {
            return await _siteSliderRepository.FilterSlider(filterSlider);
        }
        #endregion

        #region CreateSlider
        public async Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider, IFormFile sliderImage)
        {
            if (await _siteSliderRepository.CheckImageDisplayPrority(createSlider.ImagepDisplayPrority))
                return CreateSliderResult.DisplayProrityIsExist;
            var newSlider = new Slider()
            {
                DisplayPrority = createSlider.ImagepDisplayPrority,
                IsActive = createSlider.IsActive,
                Link = createSlider.Link
            };
            try
            {
                if (sliderImage != null && sliderImage.IsImage())
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(sliderImage.FileName);
                    var res = sliderImage.AddImageToServer(imageName, PathExtension.SliderImageOriginServer, 100, 100,
                        PathExtension.SliderImageThumbServer);
                    if (res)
                    {
                        newSlider.ImageName = imageName;
                    }
                }
                else
                {
                    return CreateSliderResult.ImageError;
                }
                await _siteSliderRepository.AddSlider(newSlider);
                await _siteSliderRepository.Save();
                return CreateSliderResult.Sucess;
            }
            catch
            {
                return CreateSliderResult.Error;
            }
        }
        #endregion

        #region DeleteSlider
        public async Task<bool> DeleteSlider(int sliderId)
        {
            var slider = await _siteSliderRepository.GetSliderBySliderId(sliderId);
            if (slider == null) return false;
            try
            {
                slider.IsDelete = true;
                _siteSliderRepository.UpdateSlider(slider);
                await _siteSliderRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region RestoreSlider
        public async Task<bool> RestoreSlider(int sliderId)
        {
            var slider = await _siteSliderRepository.GetSliderBySliderId(sliderId);
            if (slider == null) return false;
            try
            {
                slider.IsDelete = false;
                _siteSliderRepository.UpdateSlider(slider);
                await _siteSliderRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SliderInfoForEdit
        public async Task<EditSliderViewModel> SliderInfoForEdit(int sliderId)
        {
            var slider = await _siteSliderRepository.GetSliderBySliderId(sliderId);
            if (slider == null) return null;
            return new EditSliderViewModel()
            {
                SliderId = sliderId,
                IsActive = slider.IsActive,
                Link = slider.Link,
                ImagepDisplayPrority = slider.DisplayPrority.Value,
                ImageName = slider.ImageName
                
            };
        }
        #endregion

        #region EditSlider
        public async Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider, IFormFile sliderImage)
        {
            var mainSlider = await _siteSliderRepository.GetSliderBySliderId(editSlider.SliderId);
            if (mainSlider == null) return EditSliderResult.NotFound;
            if (await _siteSliderRepository.CheckImageDisplayProrityForEdit(editSlider.SliderId,
                editSlider.ImagepDisplayPrority)) return EditSliderResult.DisplayProrityIsExist;
            mainSlider.DisplayPrority = editSlider.ImagepDisplayPrority;
            mainSlider.IsActive = editSlider.IsActive;
            mainSlider.Link = editSlider.Link;
            try
            {
                if (sliderImage != null)
                {
                    if (sliderImage.IsImage())
                    {
                        var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(sliderImage.FileName);
                        var res = sliderImage.AddImageToServer(imageName, PathExtension.SliderImageOriginServer, 100, 100,
                            PathExtension.SliderImageThumbServer, mainSlider.ImageName);
                        if (res)
                        {
                            mainSlider.ImageName = imageName;
                        }
                        else
                        {
                            return EditSliderResult.ImageError;
                        }
                    }
                    else
                    {
                        return EditSliderResult.ImageError;
                    }
                }
              
                 _siteSliderRepository.UpdateSlider(mainSlider);
                await _siteSliderRepository.Save();
                return EditSliderResult.Sucess;
            }
            catch
            {
                return EditSliderResult.Error;
            }
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
            await _settingRepository.DisposeAsync();
            await _siteSliderRepository.DisposeAsync();
        }
        #endregion

    }
}
