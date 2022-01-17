using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Account
{
    public class InformationUserForSidebarViewModel
    {
        [Display(Name = "شماره تماس")]
        public string Mobile { get; set; }
        public string UserAvatar { get; set; }
    }
}
