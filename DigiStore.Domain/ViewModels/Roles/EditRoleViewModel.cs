namespace DigiStore.Domain.ViewModels.Roles
{
   public class EditRoleViewModel:CreateRoleViewModel
    {
        public int RoleId { get; set; }
    }

   public enum EditRoleResult
   {
       Success,
       Error
   }
}
