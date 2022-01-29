using System;

namespace DigiStore.Domain.ViewModels.User
{
    public class UserDetialViewModel : CreateUserViewModel
    {
        public bool IsBlock { set; get; }
        public bool IsDelete { set; get; }
        public DateTime RegisterDate { set; get; }
        public string UserAvatar { set; get; }
        public string FullName { get; set; }

    }
}
