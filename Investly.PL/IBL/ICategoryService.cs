using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface ICategoryService
    {
        public List<CategoryForListDto> GetAllCategories();
        public List<CategoryDto> GetAllCategoriesWithStatus();
        public CategoryDtoWithPagination GetPaginatedCategories(CategorySearchDto CategorySearch);
        public int AddCategory(AddCategoryWithStandardsDto categoryDto, int? loggedInUser);
        public int UpdateCategoryWithStandards(UpdateCategoryWithStandardsDto categoryDto, int? loggedInUser);
        public int ChangeCategoryStatus(int categoryId, int status, int? loggedInUser);
        public CategoryTotalActiveInactiveDto GetTotalActiveInactiveCategories();






    }
}