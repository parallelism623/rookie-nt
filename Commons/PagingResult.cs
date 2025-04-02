﻿namespace mvc_todolist.Commons
{
    public class PagingResult<T> where T : class
    {
        public PagingResult() {}

        
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        
        
    }
}