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
    }
}
