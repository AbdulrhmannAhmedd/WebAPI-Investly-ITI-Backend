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

        [HttpGet("with-status")]
        public IActionResult GetAllCategoriesWithStatus()
        {
            var res = _categoryService.GetAllCategoriesWithStatus();
            if (res != null)
            {
                return Ok(new ResponseDto<List<CategoryDto>>
                {
                    IsSuccess = true,
                    Message = "Categories retrieved successfully.",
                    Data = res,
                    StatusCode = StatusCodes.Status200OK
                });

            }
            else
            {
                return NotFound(new ResponseDto<List<CategoryDto>>
                {
                    IsSuccess = false,
                    Message = "Categories retrieved failed.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

        }

        [HttpPost("Paginated")]
        public IActionResult GetPaginatedCategories(CategorySearchDto categorySearch)
        {
            var res = _categoryService.GetPaginatedCategories(categorySearch);

            if (res != null)
            {
                return Ok(new ResponseDto<CategoryDtoWithPagination>
                {
                    IsSuccess = true,
                    Message = "Paginated Categories retrieved successfully.",
                    Data = res,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else
            {
                return NotFound(new ResponseDto<CategoryDtoWithPagination>
                {
                    IsSuccess = false,
                    Message = "Paginated Categories retrieved failed.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
        }

        [HttpPost("AddWithStandards")]
        public IActionResult AddCategoryWithStandards([FromBody] AddCategoryWithStandardsDto category)
        {
            var res = _categoryService.AddCategory(category, User.GetUserId());

            if (res > 0)
            {
                return Created($"api/Category/{res}", new ResponseDto<CategoryDto>
                {
                    IsSuccess = true,
                    Message = "Category created successfully.",
                    Data = null,
                    StatusCode = StatusCodes.Status201Created
                });

            }
            else if (res == -1)
            {
                return BadRequest(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Category already exists.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            else
            {
                return BadRequest(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Category creation failed.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPut("UpdateWithStandards")]
        public IActionResult UpdateCategoryWithStandards([FromBody] UpdateCategoryWithStandardsDto category)
        {
            var res = _categoryService.UpdateCategoryWithStandards(category, User.GetUserId());

            if (res > 0)
            {
                return Ok(new ResponseDto<CategoryDto>
                {
                    IsSuccess = true,
                    Message = "Category updated successfully.",
                    Data = null,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else if (res == -1)
            {
                return BadRequest(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Category does not exist.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            else if (res == -2)
            {
                return BadRequest(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Category already exists.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            else
            {
                return BadRequest(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Category update failed.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

        }

        [HttpPut("ChangeStatus/{categoryId}/{status}")]
        public IActionResult ChangeCategoryStatus(int categoryId, int status)
        {
            var res = _categoryService.ChangeCategoryStatus(categoryId, status, User.GetUserId());
            if (res > 0)
            {
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Category status changed successfully.",
                    Data = null,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else if (res == -1)
            {
                return NotFound(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Category not found.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            else
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Failed to change category status.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }


        }

        [HttpGet("total-active-inactive")]
        public ResponseDto<CategoryTotalActiveInactiveDto> GetTotalActiveInactive()
        {
            var total = _categoryService.GetTotalActiveInactiveCategories();
            return new ResponseDto<CategoryTotalActiveInactiveDto>
            {
                IsSuccess = true,
                Message = "Total active and inactive categories retrieved successfully.",
                Data = total,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
