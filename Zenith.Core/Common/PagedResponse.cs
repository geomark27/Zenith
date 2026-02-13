namespace Zenith.Core.Common
{
    public class PagedResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public static PagedResponse<T> SuccessResponse(T data, int totalCount, int pageNumber, int pageSize)
        {
            return new PagedResponse<T>
            {
                Success = true,
                Message = "Success",
                Data = data,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}