using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Web.Http;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DigiStore.Web.Areas.Seller.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ProductController : SellerBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        private readonly IBranadService _branadService;
        public ProductController(IProductService productService, ISellerService sellerService, IBranadService branadService)
        {
            _productService = productService;
            _sellerService = sellerService;
            _branadService = branadService;
        }
        #endregion

        #region ProductList
        [HttpGet("Products-list")]
        public async Task<IActionResult> Index(FilterProductViewModel filterProduct)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            filterProduct.SellerId = seller.SellerId;
            filterProduct.TakeEntity = 10;
            var result = await _productService.FilterProduct(filterProduct);
            return View(result);
        }
        #endregion

        #region Create Product
        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
            ViewBag.Brand = await _branadService.GetAllBrands();
            return View();
        }
        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createProduct, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.CreateProduc(createProduct, seller.SellerId, ProductImage);
                switch (res)
                {
                    case CreateProductResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت محصول";
                        break;
                    case CreateProductResult.HasNoImage:
                        TempData[WarningMessage] = "تصویر محصول جدید را وارد نمایید";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = $"محصول {createProduct.Title}  با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
            ViewBag.Brand = await _branadService.GetAllBrands();
            return View(createProduct);
        }
        #endregion

        #region EditProduct
        [HttpGet("edit-product/{id}")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetProductForEdit(id);
            if (product == null) return NotFound();
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
            ViewBag.Brand = await _branadService.GetAllBrands();
            return View(product);
        }
        [HttpPost("edit-product/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, EditProductViewModel editProduct, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProduct(editProduct, User.GetUserId(), ProductImage);
                switch (res)
                {
                    case EditProductResult.Erorr:
                        TempData[ErrorMessage] = "خطا در ویرایش محصول مورد نظر";
                        break;

                    case EditProductResult.NotFoundProduct:
                        TempData[ErrorMessage] = "محصولی با این مشحصات یافت نشد";
                        break;
                    case EditProductResult.ImageIsNotValid:
                        TempData[WarningMessage] = "تصویر بارگذاری شده معتبر نمی باشد";
                        break;
                    case EditProductResult.NotFoundUser:
                        TempData[ErrorMessage] = "فروشگاهی با این کاربر یافت نشد";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "محصول مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
            ViewBag.Brand = await _branadService.GetAllBrands();
            return View(editProduct);
        }
        #endregion

        #region ProductGallery
        [HttpGet("product-galleries/{id}")]
        public async Task<IActionResult> GetProductGalleries(int id)
        {
            ViewBag.ProductId = id;
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            return View(await _productService.GetAllProductGalleryForSellerpanel(id, seller.SellerId));
        }
        #endregion

        #region CreateProductGallery

        [HttpGet("create-product-gallery/{id}")]
        public async Task<IActionResult> createProductGallery(int id)
        {
            var product = await _productService.GetProductBySellerOwnerId(id, User.GetUserId());
            if (product == null) return NotFound();
            ViewBag.product = product;
            return View();
        }
        [HttpPost("create-product-gallery/{id}")]
        public async Task<IActionResult> createProductGallery(int id, CreateProductGalleryViewModel createProductGallery, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.CreateProductGallery(createProductGallery, id, seller.SellerId, ProductImage);
                switch (res)
                {
                    case CreateProductGalleryResult.Error:
                        TempData[ErrorMessage] = "خطا در فرایند ثبت تصویر";
                        break;
                    case CreateProductGalleryResult.ProductNotFound:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد.";
                        break;
                    case CreateProductGalleryResult.ImageIsNull:
                        TempData[ErrorMessage] = "تصویر مربوطه را وارد نمایید";
                        break;
                    case CreateProductGalleryResult.DisplayProrityIsExist:
                        TempData[WarningMessage] = "تصویری با این اولویت وجود دارد";
                        break;
                    case CreateProductGalleryResult.NotForUserProduct:
                        TempData[ErrorMessage] = "این محصول مربوط به فروشگاه شما نمی باشد";
                        break;
                    case CreateProductGalleryResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت گالری با موفقیت انجام شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = id });

                }
            }
            var product = await _productService.GetProductBySellerOwnerId(id, User.GetUserId());
            if (product == null) return NotFound();
            ViewBag.product = product;
            return View(createProductGallery);
        }
        #endregion

        #region EditProductGallery
        [HttpGet("product_{productId}/edit-gallery/{id}")]
        public async Task<IActionResult> EditGallery(int productId, int id)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            var mainGallery = await _productService.GetEditProductGalleryForEdit(id, seller.SellerId);
            if (mainGallery == null) return NotFound();
            return View(mainGallery);
        }
        [HttpPost("product_{productId}/edit-gallery/{id}")]
        public async Task<IActionResult> EditGallery(int productId, int id, EditOrDeleteProductGalleryViewModel editProductGallery, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.EditProductGallery(editProductGallery, id, seller.SellerId, ProductImage);
                switch (res)
                {
                    case EditOrDeleteProductGalleryResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش تصویر گالری";
                        break;
                    case EditOrDeleteProductGalleryResult.GalleryNotFound:
                        TempData[ErrorMessage] = "تصویر مورد نظر یافت نشد";
                        break;
                    case EditOrDeleteProductGalleryResult.DisplayProrityIsExist:
                        TempData[WarningMessage] = "تصویری با این اولویت وجود دارد";
                        break;
                    case EditOrDeleteProductGalleryResult.NotForUserProduct:
                        TempData[ErrorMessage] = "این محصول مربوط به فروشگاه شما نمی باشد";
                        break;
                    case EditOrDeleteProductGalleryResult.Success:
                        TempData[SuccessMessage] = "تصویر مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = productId });
                }
            }
            return View(editProductGallery);
        }
        #endregion

        #region DeleteProductGallery
        [HttpPost]
        public async Task<IActionResult> DeleteProductGallery(EditOrDeleteProductGalleryViewModel deleteProductGaller)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.DeleteProductGallery(deleteProductGaller, deleteProductGaller.GalleryId,
                    seller.SellerId);
                switch (res)
                {
                    case EditOrDeleteProductGalleryResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش تصویر گالری";
                        break;
                    case EditOrDeleteProductGalleryResult.GalleryNotFound:
                        TempData[ErrorMessage] = "تصویر مورد نظر یافت نشد";
                        break;
                    case EditOrDeleteProductGalleryResult.NotForUserProduct:
                        TempData[ErrorMessage] = "این محصول مربوط به فروشگاه شما نمی باشد";
                        break;
                    case EditOrDeleteProductGalleryResult.Success:
                        TempData[SuccessMessage] = "تصویر مورد نظر با موفقیت  بازگردانده شذ شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = deleteProductGaller.ProductId });

                }
            }
            return RedirectToAction("GetProductGalleries", "Product", new { id = deleteProductGaller.ProductId });
        }
        #endregion

        #region RestoreProductGallery
        [HttpPost("RestoreProductGallery")]
        public async Task<IActionResult> RestoreProductGallery(EditOrDeleteProductGalleryViewModel deleteProductGaller)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.ResotrProductGallery(deleteProductGaller, deleteProductGaller.GalleryId,
                    seller.SellerId);
                switch (res)
                {
                    case EditOrDeleteProductGalleryResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش تصویر گالری";
                        break;
                    case EditOrDeleteProductGalleryResult.GalleryNotFound:
                        TempData[ErrorMessage] = "تصویر مورد نظر یافت نشد";
                        break;
                    case EditOrDeleteProductGalleryResult.NotForUserProduct:
                        TempData[ErrorMessage] = "این محصول مربوط به فروشگاه شما نمی باشد";
                        break;
                    case EditOrDeleteProductGalleryResult.Success:
                        TempData[SuccessMessage] = "تصویر مورد نظر با موفقیت بازگردانی شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = deleteProductGaller.ProductId });

                }
            }
            return RedirectToAction("GetProductGalleries", "Product", new { id = deleteProductGaller.ProductId });
        }
        #endregion
    }
}
