using Investly.PL.BL;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult GetAllCatgeories()
        {
            var res = _categoryService.GetAllCategories();
            if (res != null)
                return Ok(new ResponseDto<List<CategoryForListDto>>
                {
                    IsSuccess = true,
                    Message = "Categories retrieved successfully.",
                    Data = res,
                    StatusCode = StatusCodes.Status200OK
                });

            else
                return NotFound(new ResponseDto<List<CategoryForListDto>>
                {
                    IsSuccess = false,
                    Message = "Categories retrieved failed.",
                    Data = res,
                    StatusCode = StatusCodes.Status404NotFound
                });
        }
    }
}
