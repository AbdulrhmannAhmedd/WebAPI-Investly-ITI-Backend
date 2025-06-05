﻿namespace Investly.PL.Dtos
{
    public class PaginatedResultDto<T> where T : class
    {
       
            public List<T> Items { get; set; } = new();
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
            public int TotalFilteredItems { get; set; }
            public int TotalItemsInTable { get; set; }
            public bool HasNextPage => CurrentPage < TotalPages;
            public bool HasPreviousPage => CurrentPage > 1;
        
    }
}
