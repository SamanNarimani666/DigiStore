using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Domain.ViewModels.Brand
{
    public class EditBrandViewModel:CreateBrandViewModel
    {
        public int BrandId { get; set; }
        public string BrandLogo { get; set; }
    }
    public enum EditBrandResult
    {
        Success,
        NotFount,
        Error
    }
}
