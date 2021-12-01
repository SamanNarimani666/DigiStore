namespace DigiStore.Application.Security.PassWordHashing
{
    public interface IPasswordHelper
    {
        string EncodePasswordMd5(string password);
    }
}
