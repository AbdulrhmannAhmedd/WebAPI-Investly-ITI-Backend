namespace Investly.PL.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; } 

        public List<StandardCategoryDto> standardCategoryDto { get; set; } = new List<StandardCategoryDto>();

    }

    public class CategorySearchDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchInput { get; set; }
        public int status { get; set; }
    }

    public class CategoryDtoWithPagination
    {
        public List<CategoryDto> Items { get; set; } = new List<CategoryDto>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }

    public class AddNewStandardWithWeightDto
    {
        public string StandardName { get; set; }
        public string FormQuestion { get; set; }
        public int Weight { get; set; }
    }

    public class AddCategoryWithStandardsDto
    {
        public string Name { get; set; }
        public List<AddNewStandardWithWeightDto> Standards { get; set; } = new List<AddNewStandardWithWeightDto>();
    }

    public class UpdateCategoryWithStandardsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UpdateStandardWithWeightDto> Standards { get; set; } = new List<UpdateStandardWithWeightDto>();
    }

    public class UpdateStandardWithWeightDto
    {
        public int? StandardId { get; set; } 
        public string StandardName { get; set; }
        public string FormQuestion { get; set; }
        public int Weight { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class CategoryTotalActiveInactiveDto
    {
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
    }


}
