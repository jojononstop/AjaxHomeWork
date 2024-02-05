namespace Ajax0122.Models.Dtos
{
    public class SerchDtos
    {    //搜尋相關
        public string? keyword { get; set; }
        public int? categoryId { get; set; } = 0;

        //排序相關
        public string? sortBy { get; set; }
        public string? sortType { get; set; } = "asc";

        //分頁相關
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 9; //一頁9筆

    }
}
