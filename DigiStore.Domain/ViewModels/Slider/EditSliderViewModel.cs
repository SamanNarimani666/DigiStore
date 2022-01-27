
namespace DigiStore.Domain.ViewModels.Slider
{
    public class EditSliderViewModel : CreateSliderViewModel
    {
        public int SliderId { get; set; }
        public string ImageName { get; set; }
    }
    public enum EditSliderResult
    {
        Sucess,
        ImageError,
        DisplayProrityIsExist,
        NotFound,
        Error
    }
}
