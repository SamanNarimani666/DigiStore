using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Category;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.ProductComment;
using DigiStore.Web.Http;
using Microsoft.AspNetCore.Mvc;


namespace DigiStore.Web.Areas.Admin.Controllers
{
    [Route("Product")]
    [AutoValidateAntiforgeryToken]
    public class ProductController : AdminBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region ProductList
        [HttpGet("ProductList")]
        public async Task<IActionResult> Index(FilterProductViewModel filterProduct)
        {
            filterProduct.SellerId = null;
            return View(await _productService.FilterProduct(filterProduct));
        }
        #endregion

        #region Accept Product
        public async Task<IActionResult> AcceptSellerProduct(int id)
        {
            var result = await _productService.AcceptSellerProduct(id);
            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت تایید شد", null);

            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "محصول مورد نظر یافت نشد", null);
        }
        #endregion

        #region reject Seller Product
        [HttpGet("RejectSellerProduct/{id}")]
        public async Task<IActionResult> RejectSellerProduct(int id)
        {
            return PartialView(await _productService.GetPorductInfoForReject(id));
        }
        [HttpPost("RejectSellerProduct/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerProduct(RejectProductViewModel rejectProduct)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.RejectSellerProduct(rejectProduct);
                if (result)
                {
                    TempData[SuccessMessage] = "محصول مورد نظر با موفقیت رد شد";
                    return RedirectToAction("Index");

                }
                TempData[SuccessMessage] = "محصولی با این مشخصات";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        #endregion

        #region ProductDetial
        [HttpGet("product-detial/{productId}")]
        public async Task<IActionResult> ProductDetilas(int productId)
        {
            var product = await _productService.GetProductDetail(productId);
            if (product == null) return NoContent();
            return View(product);
        }
        #endregion

        #region ProductCategory

        [HttpGet("product-category")]
        public async Task<IActionResult> ProductCategory()
        {
            return View(await _productService.GetAllActiveProductCategory());
        }
        #endregion

        #region EditCategory
        [HttpGet("edit-category/{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId)
        {
            return PartialView(await _productService.CategoryInfoForEdit(categoryId));
        }
        [HttpPost("edit-category/{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId, EditCategoryViewModel editCategory)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProductCategory(editCategory);
                switch (res)
                {
                    case EditCategoryResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش اطلاعات";
                        return RedirectToAction("ProductCategory");
                    case EditCategoryResult.NotFound:
                        TempData[ErrorMessage] = "دسته ایی با این مشخصات یافت نشد";
                        return RedirectToAction("ProductCategory");
                    case EditCategoryResult.Success:
                        TempData[SuccessMessage] = "دسته با موفقیت ویرایش شد";
                        return RedirectToAction("ProductCategory");
                }
            }
            return PartialView(await _productService.CategoryInfoForEdit(categoryId));
        }

        #endregion

        #region CreateCategory
        [HttpGet("create-category")]
        public IActionResult CreateCategory()
        {
            return PartialView();
        }
        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel createCategory)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateCategoryProduct(createCategory);
                switch (res)
                {
                    case CreateCategoryResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت اطلاعات";
                        return RedirectToAction("ProductCategory");
                    case CreateCategoryResult.Success:
                        TempData[SuccessMessage] = "دسته با موفقیت ثبت شد";
                        return RedirectToAction("ProductCategory");
                }
            }
            return PartialView(createCategory);
        }
        #endregion

        #region CreateSubCategory
        [HttpGet("create-subcategory/{parentId}")]
        public async Task<IActionResult> CreateSubCategory(int parentId)
        {
            ViewBag.ParenTitle = await _productService.GetProductCategoryByCategoryId(parentId);
            return PartialView(new CreateSubCategoryViewModel() { ParentId = parentId });
        }
        [HttpPost("create-subcategory/{parentId}")]
        public async Task<IActionResult> CreateSubCategory(int parentId, CreateSubCategoryViewModel createSubCategory)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateSubCategoryProduct(createSubCategory);
                switch (res)
                {
                    case CreateCategoryResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت اطلاعات";
                        return RedirectToAction("ProductCategory");
                    case CreateCategoryResult.Success:
                        TempData[SuccessMessage] = "زیر دسته با موفقیت ثبت شد";
                        return RedirectToAction("ProductCategory");
                }
            }
            return PartialView(createSubCategory);
        }
        #endregion

        #region DeleteCategory
        [HttpGet("deleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var res = await _productService.DeleteProductCategory(categoryId);
            if (res)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "دسته مورد نظر با موفقیت حذف شد", null);

            }
            else
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "خطا در حذف دسته", null);

            }
        }
        #endregion

        #region ProductComments

        [HttpGet("product-comment/{productId}")]
        public async Task<IActionResult> ProductComments(int productId, FilterProductCommentViewModel filterProductComments)
        {
            ViewBag.Product = await _productService.GetProductByProductId(productId);
            if (ViewBag.Product == null) return NoContent();

            filterProductComments.ProductId = productId;
            filterProductComments.OrderBydate = OrderBydate.Desc_Date;
            var producrComment = await _productService.filterFilterProductComment(filterProductComments);
            if (producrComment == null) return NoContent();
            return View(producrComment);
        }
        #endregion
    }
}
