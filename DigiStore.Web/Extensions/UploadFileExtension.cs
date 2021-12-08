
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Web.Extensions
{
    public static class UploadFileExtension
    {
        public static void AddFileToServer(this IFormFile File, string fileName, string orginalPath, string deletefileName=null)
        {
            if (File != null)
            {
                if (!Directory.Exists(orginalPath))
                    Directory.CreateDirectory(orginalPath);

                if (!string.IsNullOrEmpty(deletefileName))
                {

                    if (System.IO.File.Exists(orginalPath + deletefileName))
                        System.IO.File.Delete(orginalPath + deletefileName);

                }

                string OriginPath = orginalPath + fileName;

                using (var stream = new FileStream(OriginPath, FileMode.Create))
                {
                    if (!Directory.Exists(OriginPath)) File.CopyTo(stream);
                }
            }
        }
    }
}
