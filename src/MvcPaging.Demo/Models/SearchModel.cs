namespace MvcPaging.Demo.Models
{
    public class SearchModel
    {
        public int? page { get; set; }

        public SearchModel()
        {
            page = 1;
        }
    }
}
