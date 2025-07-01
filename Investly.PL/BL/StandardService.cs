using AutoMapper;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;

namespace Investly.PL.BL
{
    public class StandardService : IStandardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       public StandardService(IUnitOfWork unitOfWork,IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<StandardDto> GetStandardsByCategory(int Category_ID)
        {
            try
            {
                if (Category_ID < 0)
                {
                    return null;
                }
                var res=_unitOfWork.StandardRepo.GetAll(s=>s.CategoryStandards.Any(c=>c.CategoryId == Category_ID), "CategoryStandards");
                if(res==null)
                {
                    return null;
                }
                var standards=_mapper.Map<List<StandardDto>>(res);
                return standards;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<StandardCategoryDto> GetStandardsCategories(int CategoryId)
        {

            try
            {
                if (CategoryId < 0) {
                    return null;
                }
                var res = _unitOfWork.StandardRepo.categoryStandards(CategoryId).Select(s=>new StandardCategoryDto
                {
                    Id=s.Id,
                    StandardId=s.Standard.Id,
                    StandardName = s.Standard.Name,
                    Question = s.Standard.FormQuestion,
                    StandardCategoryWeight = s.Weight
                });
                return res.ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
