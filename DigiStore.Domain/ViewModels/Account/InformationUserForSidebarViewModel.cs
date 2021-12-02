using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Account
{
    public class InformationUserForSidebarViewModel
    {
        [Display(Name = "شماره تماس")]
        public string Mobile { get; set; }
        [Display(Name = "کیف پول")]
        public int Wallet { get; set; }
        public string UserAvatar { get; set; }
    }
}
