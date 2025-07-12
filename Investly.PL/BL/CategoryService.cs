using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
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
                var res = _unitOfWork.CategoryRepo.GetAll(c=>c.Status!=(int)CategoryStatus.deleted);
                var categories=_mapper.Map<List<CategoryForListDto>>(res);
                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CategoryDto> GetAllCategoriesWithStatus()
        {
            try
            {
                var res = _unitOfWork.CategoryRepo.GetAll();
                var categories = _mapper.Map<List<CategoryDto>>(res);
                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CategoryDtoWithPagination GetPaginatedCategories(CategorySearchDto CategorySearch)
        {
            try
            {
                var categoryQuery = _unitOfWork.CategoryRepo.FindAll(
                    item => (
                    (String.IsNullOrEmpty(CategorySearch.SearchInput) ||
                    (item.Name.Contains(CategorySearch.SearchInput))) &&
                    (CategorySearch.status == 0 || item.Status == CategorySearch.status)
                    //&&item.Status != (int)CategoryStatus.deleted
                    )
                    , properties: "CategoryStandards.Standard"
                ).OrderByDescending(c => c.CreatedAt);

                int skip = (CategorySearch.PageSize * (CategorySearch.PageNumber > 0 ? CategorySearch.PageNumber - 1 : 1));

                var paginatedData = categoryQuery
                                     .Skip(skip)
                                     .Take(CategorySearch.PageSize)
                                     .ToList();

                return new CategoryDtoWithPagination
                {
                    Items = _mapper.Map<List<CategoryDto>>(paginatedData),
                    TotalCount = categoryQuery.Count(),
                    PageSize = CategorySearch.PageSize,
                    CurrentPage = CategorySearch.PageNumber
                };

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public int AddCategory(AddCategoryWithStandardsDto categoryDto, int? loggedInUser)
        {
            int res = 0;

            try
            {
                if (categoryDto == null)
                {
                    return 0; // Invalid Input
                }

                var existedCategory = _unitOfWork.CategoryRepo.FindAll(c => c.Name == categoryDto.Name).FirstOrDefault();
                if (existedCategory != null)
                {
                    return -1; // Category Exists
                }

                var newCategory = _mapper.Map<Category>(categoryDto);

                newCategory.CreatedBy = loggedInUser;
                newCategory.CreatedAt = DateTime.UtcNow;
                newCategory.Status = (int)CategoryStatus.Active;

                _unitOfWork.CategoryRepo.Insert(newCategory);
                res = _unitOfWork.Save();

                if (res > 0)
                {
                    if(categoryDto.Standards != null && categoryDto.Standards.Any())
                    {
                        foreach ( var standard in categoryDto.Standards)
                        {
                            var newStandard = new Standard
                            {
                                Name = standard.StandardName,
                                FormQuestion = standard.FormQuestion,
                                Status = (int)StandardStatus.Active,
                                CreatedBy = loggedInUser,
                                CreatedAt = DateTime.UtcNow
                            };

                            _unitOfWork.StandardRepo.Insert(newStandard);
                            res = _unitOfWork.Save();

                            if (res >0)
                            {
                                var categoryStandard = new CategoryStandard
                                {
                                    CategoryId = newCategory.Id,
                                    StandardId = newStandard.Id,
                                    Weight = standard.Weight,
                                    Starus = (int)CategoryStatus.Active,
                                    CreatedBy = loggedInUser,
                                    CreatedAt = DateTime.UtcNow
                                };

                                _unitOfWork.CategoryStandardRepo.Insert(categoryStandard);
                                res = _unitOfWork.Save();
                            }

                        }
                    }

                    return newCategory.Id; // Category added successfully

                }
                else
                {
                    return -2; // Save failed
                }

            }
            catch(Exception ex)
            {
                return -3; // Exception
            }

        }

        public int UpdateCategoryWithStandards(UpdateCategoryWithStandardsDto categoryDto, int? loggedInUser)
        {
            int res = 0;

            try
            {

                if (categoryDto == null || categoryDto.Id == 0)
                {
                    return 0; // invalid input
                }

                // check if the category exists first
                var exisitingCategory = _unitOfWork.CategoryRepo.FirstOrDefault(
                    c => c.Id == categoryDto.Id,
                    "CategoryStandards.Standard"
                );

                if (exisitingCategory == null)
                {
                    return -1; // category not found
                }

                // check if name of the category already exists and execluding the current category
                var nameExists = _unitOfWork.CategoryRepo.FindAll(
                    c => c.Name == categoryDto.Name && c.Id != categoryDto.Id
                ).Any();

                if (nameExists)
                {
                    return -2; // Category name already exists
                }

                // Update category info
                exisitingCategory.Name = categoryDto.Name;
                exisitingCategory.UpdatedBy = loggedInUser;
                exisitingCategory.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.CategoryRepo.Update(exisitingCategory);
                res = _unitOfWork.Save();

                if (res > 0 && categoryDto.Standards != null)
                {
                    foreach(var standardDto in categoryDto.Standards)
                    {
                        if(standardDto.IsDeleted && standardDto.StandardId.HasValue) // if the standard is marked as deleted, delete the standard and the category standard
                        {
                            var categoryStandardToDelete = exisitingCategory.CategoryStandards.FirstOrDefault(
                                cs => cs.StandardId == standardDto.StandardId.Value);

                            if (categoryStandardToDelete != null)
                            {
                                _unitOfWork.CategoryStandardRepo.Remove(categoryStandardToDelete.Id);
                                _unitOfWork.StandardRepo.Remove(standardDto.StandardId.Value);
                                res = _unitOfWork.Save();
                            }

                        }
                        else if(standardDto.StandardId.HasValue) // if standardId is provided, update the existing standard and category standard
                        {
                            var standardToUpdate = _unitOfWork.StandardRepo.FirstOrDefault(
                                s => s.Id == standardDto.StandardId.Value
                            );

                            if(standardToUpdate !=null)
                            {
                                standardToUpdate.Name = standardDto.StandardName;
                                standardToUpdate.FormQuestion = standardDto.FormQuestion;
                                standardToUpdate.UpdatedBy = loggedInUser;
                                standardToUpdate.UpdatedAt = DateTime.UtcNow;

                                _unitOfWork.StandardRepo.Update(standardToUpdate);
                                res = _unitOfWork.Save();

                                var categoryStandardToUpdate = exisitingCategory.CategoryStandards.FirstOrDefault(
                                    cs => cs.StandardId == standardDto.StandardId.Value);

                                if(categoryStandardToUpdate != null)
                                {
                                    categoryStandardToUpdate.Weight = standardDto.Weight;
                                    categoryStandardToUpdate.UpdatedBy = loggedInUser;
                                    categoryStandardToUpdate.UpdatedAt = DateTime.UtcNow;

                                    _unitOfWork.CategoryStandardRepo.Update(categoryStandardToUpdate);
                                    res = _unitOfWork.Save();
                                }

                            }

                        }

                        else // If standardId is not provided, create a new standard then create a new category standard with the new standard and the existing category
                        {
                            var newStandard = new Standard
                            {
                                Name = standardDto.StandardName,
                                FormQuestion = standardDto.FormQuestion,
                                Status = (int)StandardStatus.Active,
                                CreatedBy = loggedInUser,
                                CreatedAt = DateTime.UtcNow
                            };

                            _unitOfWork.StandardRepo.Insert(newStandard);
                            res = _unitOfWork.Save();

                            if(res > 0)
                            {
                                var newCategoryStandard = new CategoryStandard
                                {
                                    CategoryId = exisitingCategory.Id,
                                    StandardId = newStandard.Id,
                                    Weight = standardDto.Weight,
                                    Starus = (int)CategoryStatus.Active,
                                    CreatedBy = loggedInUser,
                                    CreatedAt = DateTime.UtcNow
                                };

                                _unitOfWork.CategoryStandardRepo.Insert(newCategoryStandard);
                                res = _unitOfWork.Save();

                            }
                            
                        }

                    }
                }

                return res > 0 ? exisitingCategory.Id : -3; // return the category id if updated successfully, otherwise return -3 for save failed

            }
            catch (Exception ex)
            {
                return -4; // Exception 
            }
        }

        public int ChangeCategoryStatus(int categoryId, int status, int? loggedInUser)
        {
            try
            {
                int res = 0;
                var category = _unitOfWork.CategoryRepo.FirstOrDefault(c => c.Id == categoryId);
                if (category == null)
                {
                    return -1; // Category not found
                }
                category.Status = status;
                category.UpdatedBy = loggedInUser;
                category.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.CategoryRepo.Update(category);
                res = _unitOfWork.Save();
                return res;
            }
            catch (Exception ex)
            {
                return -2; // Exception occurred
            }
        }

        public CategoryTotalActiveInactiveDto GetTotalActiveInactiveCategories()
        {
            try
            {
                var result = _unitOfWork.CategoryRepo.GetTotalActiveInactive((int)(CategoryStatus.Active), (int)(CategoryStatus.Inactive));

                return new CategoryTotalActiveInactiveDto
                {
                    TotalActive = result.Item1,
                    TotalInactive = result.Item2
                };
            }
            catch (Exception ex)
            {
                return null; // Exception 
            }
        }



    }
}
