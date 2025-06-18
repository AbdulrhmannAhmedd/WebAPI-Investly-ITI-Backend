using AutoMapper;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;

namespace Investly.PL.BL
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<CategoryForListDto> GetAllCategories()
        {
            try
            {
                var res = _unitOfWork.CategoryRepo.GetAll();
                var categories=_mapper.Map<List<CategoryForListDto>>(res);
                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
