using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
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
        [HttpPost("edit-product/{id}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id,EditProductViewModel editProduct, IFormFile ProductImage )
        {

            if (ModelState.IsValid)
            {
               var res= await _productService.EditProduct(editProduct, User.GetUserId(), ProductImage);
               switch (res)
               {
                    case EditProductResult.Erorr:
                        TempData[ErrorMessage] = "خطا در ویرایش محصول مورد نظر";
                        break;

                    case EditProductResult.NotFoundProduct:
                        TempData[ErrorMessage] = "محصولی با این مشحصات یافت نشد";
                        break;

                    case EditProductResult.NotFoundUser:
                        TempData[ErrorMessage] = "فروشگاهی با این کاربر یافت نشد";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "محصول مورد نظر با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
            ViewBag.Brand = await _branadService.GetAllBrands();
            return View(editProduct);
        }
        #endregion

    }
}
