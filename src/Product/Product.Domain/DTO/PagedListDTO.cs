namespace Product.Domain.DTO
{
    public class PagedListDTO<TEntity>
    {
        public PagedListDTO(IEnumerable<TEntity> items, int totalCount, int totalPages, int currentPage, int pageSize,
            bool hasPrevious, bool hasNext)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
            HasPrevious = hasPrevious;
            HasNext = hasNext;
            Items = items;
        }

        public IEnumerable<TEntity> Items { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }

        public int CurrentPage { get; }

        public int PageSize { get; }

        public bool HasPrevious { get; }

        public bool HasNext { get; }

        public static PagedListDTO<TEntity> ToPagedList(IEnumerable<TEntity> items, int totalCount, int totalPages,
            int currentPage, int pageSize, bool hasPrevious, bool hasNext) => new(items, totalCount, totalPages,
            currentPage, pageSize, hasPrevious, hasNext);
    }

}
