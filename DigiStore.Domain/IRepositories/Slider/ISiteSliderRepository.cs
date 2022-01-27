using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Domain.ViewModels.Slider
{
    public interface ISiteSliderRepository:IAsyncDisposable
    {
        Task<List<Entities.Slider>> GetAllActiveSlider();
        Task<FilterSliderViewModel> FilterSlider(FilterSliderViewModel filterSlider);
        Task AddSlider(Entities.Slider slider);
        Task<bool> CheckImageDisplayPrority(byte displayPrority);
        Task Save();
    }
}
