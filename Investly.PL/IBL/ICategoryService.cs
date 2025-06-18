using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface ICategoryService
    {
        public List<CategoryForListDto> GetAllCategories();
    }
}