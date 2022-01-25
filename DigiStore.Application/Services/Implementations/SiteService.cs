using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Application.Convertors;
using DigiStore.Application.Security;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.ContactUs;
using DigiStore.Domain.IRepositories.SiteSetting;
using DigiStore.Domain.ViewModels.Contacts;
using DigiStore.Domain.ViewModels.SiteSetting;
using DigiStore.Domain.ViewModels.Slider;

namespace DigiStore.Application.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region Contrcutor
        private readonly IContactUsRepository _contactUsRepository;
        private readonly ISiteSettingRepository _settingRepository;
        private readonly ISiteSliderRepository _siteSliderRepository;
        public SiteService(IContactUsRepository contactUsRepository, ISiteSettingRepository settingRepository, ISiteSliderRepository siteSliderRepository)
        {
            _contactUsRepository = contactUsRepository;
            _settingRepository = settingRepository;
            _siteSliderRepository = siteSliderRepository;
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
